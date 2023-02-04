using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attacks by starting with a laser pointing away from the player, then sweeping
// towards the camera to "chop" the player
public class LaserEyesController : MonoBehaviour
{
    enum States
    {
        PATROL,
        AVOID,
        ATTACK
    }

    [Tooltip("How many seconds to wait before attacking")]
    public float attackFrequency = 10.0f;
    [Tooltip("Walk speed")]
    public float walkSpeed = 1.5f;
    [Tooltip("Multiplier applied when running away")]
    public float runFactor = 2.0f;
    [Tooltip("How long the laser stays still pointing away from the player, also how long it lingers in the player direction")]
    public float laserBeamLinger = 0.2f;
    [Tooltip("Put the LaserBeam prefab here")]
    public GameObject laserBeamPrefab;
    [Tooltip("How far is considererd in front")]
    public float inFrontFactor = 0.7f;
    [Tooltip("How far above position is the floor")]
    public float floorPosition = 0.26f;
    [Tooltip("Within what y difference is considered at the same level")]
    public float yThreshold = 1.0f;
    [Tooltip("When to return to patrolling, after losing sight of the player")]
    public float returnToPatrolTime = 5.0f;
    [Tooltip("Normalised time to turn laser on in attack animation")]
    public float laserOnTime = 0.33f;
    [Tooltip("Normalised time to turn laser off in attack animation")]
    public float laserOffTime = 0.66f;
    [Tooltip("Layer mask to use when checking if this enemy can see the player - make sure you exclude Enemy")]
    public LayerMask layerMask;

    public Transform target;

    private Rigidbody rb;
    private Animator animator;

    private States currentState;

    private float lastStateTransition; // Time.fixedTime when state last changed
    private bool moveRight;
    private bool laserOn;
    private bool mirrorOn;
    private Transform onionEye; // for tracking where to put the laser beam
    private Transform onionChild;

    private GameObject laserBeam;

    void SetTarget(Transform desiredTarget)
    {
        target = desiredTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Default to targeting the player
        if (target == null)
        {
            target = FindObjectOfType<Player>().transform;
        }
        rb = GetComponent<Rigidbody>();
        // The animator is in a child object so we need to use GetComponentInChildren instead of GetComponent
        animator = GetComponentInChildren<Animator>();
        onionEye = GetComponentInChildren<OnionEye>().transform;
        onionChild = transform.GetChild(0);
        lastStateTransition = Time.fixedTime;
        currentState = States.PATROL;
        // Half go right, half go left
        moveRight = Random.Range(0, 2) < 1;
        UpdateMoveDirection();
        animator.SetTrigger("StartWalking");
    }

    private void UpdateMoveDirection()
    {
        if (moveRight)
        {
            transform.right = Vector3.right;
        }
        else
        {
            transform.right = Vector3.left;
        }
    }

    // Checks if this enemy can see the player
    private bool CanSeePlayer()
    {
        RaycastHit hitInfo;
        Vector3 lookPos = new Vector3(transform.position.x, onionEye.position.y, transform.position.z);
        Vector3 direction = (target.position - lookPos).normalized;
        //Debug.DrawLine(lookPos, lookPos + direction);
        if (Physics.Raycast(lookPos, direction, out hitInfo, layerMask))
        {
            if (hitInfo.transform == target)
            {
                return true;
            }
        }
        return false;
    }

    void FixedUpdate()
    {
        if (currentState == States.PATROL)
        {
            // Patrol mode, alternate between right and left walking
            animator.speed = 1.0f;
            if (moveRight)
            {
                rb.velocity = new Vector3(walkSpeed, rb.velocity.y, 0.0f);
            }
            else
            {
                rb.velocity = new Vector3(-walkSpeed, rb.velocity.y, 0.0f);
            }
            // Check if about to hit a wall or fall off the ground
            float xCheck = inFrontFactor * transform.localScale.x;
            if (moveRight)
            {
                xCheck = transform.position.x + xCheck;
            }
            else
            {
                xCheck = transform.position.x - xCheck;
            }
            Vector3 checkPosition = new Vector3(xCheck, transform.position.y + floorPosition, transform.position.z);
            //Debug.DrawLine(checkPosition, checkPosition + new Vector3(0.0f, 1.0f, 0.0f));
            Collider[] collisions = Physics.OverlapSphere(checkPosition, 0.1f);
            bool groundInFront = false;
            bool obstacleInFront = false;
            foreach (Collider collider in collisions)
            {
                if (collider.tag == "Ground")
                {
                    groundInFront = true;
                }
                else
                {
                    obstacleInFront = true;
                }
            }
            // Swap patrol direction if about to fall or hit something
            if (!groundInFront || obstacleInFront)
            {
                // Patrol in other direction
                moveRight = !moveRight;
                UpdateMoveDirection();
            }
            if (CanSeePlayer())
            {
                // Run away!
                lastStateTransition = Time.fixedTime;
                currentState = States.AVOID;
                if (target.position.x > transform.position.x)
                {
                    moveRight = false;
                }
                else
                {
                    moveRight = true;
                }
                UpdateMoveDirection();
            }
        }
        else if (currentState == States.AVOID)
        {
            if (target.position.x > transform.position.x)
            {
                moveRight = false;
            }
            else
            {
                moveRight = true;
            }
            UpdateMoveDirection();
            animator.speed = runFactor;
            if (moveRight)
            {
                rb.velocity = new Vector3(walkSpeed * runFactor, rb.velocity.y, 0.0f);
            }
            else
            {
                rb.velocity = new Vector3(-walkSpeed * runFactor, rb.velocity.y, 0.0f);
            }
            if (CanSeePlayer())
            {
                //Debug.DrawLine(target.position, onionEye.position);
                if (Mathf.Abs(target.position.y - onionEye.position.y) <= yThreshold)
                {
                    // Player is at the same level, attack!
                    animator.ResetTrigger("StartWalking");
                    animator.SetTrigger("StartAttack");
                    laserOn = false;
                    lastStateTransition = Time.fixedTime;
                    currentState = States.ATTACK;
                    transform.right = Vector3.left;
                    if (moveRight)
                    {
                        mirrorOn = true;
                        // Mirror so that laser always sweeps towards camera
                        onionChild.localScale = new Vector3(onionChild.localScale.x, onionChild.localScale.y, -onionChild.localScale.z);
                    }
                }
                else
                {
                    // Keep running away!
                    lastStateTransition = Time.fixedTime;
                    currentState = States.AVOID;
                }
            }
            if ((Time.fixedTime - lastStateTransition) > returnToPatrolTime)
            {
                lastStateTransition = Time.fixedTime;
                currentState = States.PATROL;
            }
        }
        else if (currentState == States.ATTACK)
        {
            animator.ResetTrigger("StartAttack");
            animator.speed = 1.0f;
            AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
            if (!laserOn && (animState.normalizedTime >= laserOnTime))
            {
                laserBeam = Instantiate(laserBeamPrefab, onionEye);
                laserOn = true;
            }
            if (laserOn && (animState.normalizedTime >= laserOffTime))
            {
                Destroy(laserBeam);
                laserOn = false;
            }
            if (!animState.IsName("Attack"))
            {
                if (mirrorOn)
                {
                    // Unmirror
                    onionChild.localScale = new Vector3(onionChild.localScale.x, onionChild.localScale.y, -onionChild.localScale.z);
                }
                mirrorOn = false;
                lastStateTransition = Time.fixedTime;
                currentState = States.AVOID;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
