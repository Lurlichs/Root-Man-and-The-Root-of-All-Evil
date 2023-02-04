using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Missile type enemy that tries to launch itself in an arc at the player
public class MissileController : Enemy
{
    enum States
    {
        IDLE,
        WIGGLE,
        RISING,
        ATTACK
    }
    [Tooltip("How many seconds to wait before attacking")]
    public float attackFrequency = 10.0f;
    [Tooltip("How long to wiggle before starting to attack")]
    public float wiggleTime = 3.0f;
    [Tooltip("How fast to rise out of the ground before aiming at player")]
    public float preAttackSpeed = 10.0f;
    [Tooltip("How fast to move when aiming at the player")]
    public float attackSpeed = 20.0f;
    [Tooltip("Initial movement boost strength, higher = faster")]
    public float boostStrength = 1.0f;
    [Tooltip("Initial movement boost decay rate, higher = decaying more quickly")]
    public float boostDecay = 20.0f;
    [Tooltip("Minimum height to rise out of the ground")]
    public float preAttackHeightMin = 5.0f;
    [Tooltip("Maximum height to rise out of the ground")]
    public float preAttackHeightMax = 15.0f;
    [Tooltip("Player must be within this X distance to provoke attacks")]
    public float attackDistance = 20.0f;
    [Tooltip("How long, in seconds, to ignore collisions for when starting to attack")]
    public float attackColliderBlanking = 0.150f;

    private Rigidbody rb;
    private Animator animator;
    private Collider collider;

    private States currentState;

    private float lastStateTransition; // Time.fixedTime when state last changed
    private float preAttackHeight; // how high above the ground we've chosen to rise
    private float attackAngle; // in radian

    [Header("Leave Blank if not a boss enemy")]
    [SerializeField] private Bosses_ScriptableObj BossScObj;

    // Start is called before the first frame update
    void Start()
    {
        if (BossScObj != null)
        {
            //health = BossScObj.baseHealth;
            //UI_Manager.Instance.SetBossHealthBar(BossScObj);
        }
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        rb = GetComponent<Rigidbody>();
        // The animator is in a child object so we need to use GetComponentInChildren instead of GetComponent
        animator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
        lastStateTransition = Time.fixedTime;
        currentState = States.IDLE;
    }

    // Update is called once per frame
    void Update()
    {

    }

    float CalculateBoostFactor()
    {
        float t = Time.fixedTime - lastStateTransition;
        return boostStrength * Mathf.Exp(-boostDecay * t) + 1.0f;
    }

    void FixedUpdate()
    {
        if (currentState == States.IDLE)
        {
            // Check if time to do a new attack
            if (((Time.fixedTime - lastStateTransition) > attackFrequency)
             && (Mathf.Abs(player.transform.position.x - transform.position.x) <= attackDistance))
            {
                // Start wiggling before changing position
                animator.ResetTrigger("StopFlying");
                animator.SetTrigger("StartWiggle");
                // Start wiggling in ground
                transform.right = Vector3.down;
                lastStateTransition = Time.fixedTime;
                currentState = States.WIGGLE;
            }
        }
        else if (currentState == States.WIGGLE)
        {
            // Check if time to start rising
            if ((Time.fixedTime - lastStateTransition) > wiggleTime)
            {
                // Go higher than both current height and target
                float yBase = Mathf.Max(transform.position.y, player.transform.position.y);
                preAttackHeight = yBase + Random.Range(preAttackHeightMin, preAttackHeightMax);
                // Start a bit into the air to avoid colliding with the floor
                transform.position = new Vector3(transform.position.x, transform.position.y + 2.0f, 0.0f);
                animator.ResetTrigger("StartWiggle");
                animator.SetTrigger("StartFlying");
                lastStateTransition = Time.fixedTime;
                currentState = States.RISING;
            }
        }
        else if (currentState == States.RISING)
        {
            rb.velocity = new Vector3(0.0f, preAttackSpeed * CalculateBoostFactor(), 0.0f);
            if (transform.position.y >= preAttackHeight)
            {
                // Up to required, height, start real attack
                // Set attack angle to head to where player is now
                // Actually aim a little bit high
                attackAngle = Mathf.Atan2(player.transform.position.y - transform.position.y + 1.0f, player.transform.position.x - transform.position.x);
                lastStateTransition = Time.fixedTime;
                currentState = States.ATTACK;
                // Temporarily disable the collider in case there are nearby
                // walls blocking the carrot from turning towards the player
                collider.enabled = false;
            }
        }
        else if (currentState == States.ATTACK)
        {
            float boostedAttackSpeed = attackSpeed * CalculateBoostFactor();
            rb.velocity = new Vector3(Mathf.Cos(attackAngle) * boostedAttackSpeed, Mathf.Sin(attackAngle) * boostedAttackSpeed, 0.0f);
            if (!collider.enabled && (Time.fixedTime - lastStateTransition >= attackColliderBlanking))
            {
                // Re-enable collider as its been long enough for the carrot to travel
                // enough to clear nearby walls
                collider.enabled = true;
            }
        }
        // Lock z position
        if (currentState == States.RISING)
        {
            // Point in opposite of direction of travel
            transform.right = -rb.velocity.normalized;
        }
        else if (currentState == States.ATTACK)
        {
            // Point in direction of travel
            transform.right = rb.velocity.normalized;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState == States.ATTACK)
        {
            // Stop attacking
            animator.ResetTrigger("StartFlying");
            animator.SetTrigger("StopFlying");
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.right = Vector3.down; // plant into ground
            lastStateTransition = Time.fixedTime;
            currentState = States.IDLE;
        }
    }
}
