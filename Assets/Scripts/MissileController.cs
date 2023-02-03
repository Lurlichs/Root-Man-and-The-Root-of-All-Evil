using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Missile type enemy that tries to launch itself in an arc at the player
public class MissileController : MonoBehaviour
{
    public float attackFrequency = 10.0f;
    public float preAttackSpeed = 10.0f;
    public float attackSpeed = 20.0f;
    public float preAttackHeightMin = 5.0f;
    public float preAttackHeightMax = 15.0f;

    private float lastAttack;
    private Rigidbody rb;
    private bool inPreAttack;
    private bool inAttack;
    private float preAttackHeight;
    private float attackAngle; // in radian
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastAttack = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetTarget(Transform desiredTarget)
    {
        target = desiredTarget;
    }

    void FixedUpdate()
    {
        bool startAttack = false;

        // Check if time to do a new attack
        if ((Time.fixedTime - lastAttack) > attackFrequency)
        {
            lastAttack = Time.fixedTime;
            // Launch a new attack
            inPreAttack = true;
            // Go higher than both current height and target
            float yBase = Mathf.Max(transform.position.y, target.position.y);
            preAttackHeight = yBase + Random.Range(preAttackHeightMin, preAttackHeightMax);
            // Start a bit into the air to avoid colliding with the floor
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, 0.0f);
        }
        if (inPreAttack)
        {
            rb.velocity = new Vector3(0.0f, preAttackSpeed, 0.0f);
            if (transform.position.y >= preAttackHeight)
            {
                // Up to required, height, start real attack
                startAttack = true;
                inPreAttack = false;
                // Set attack angle to head to where player is now
                // Actually aim a little bit high
                attackAngle = Mathf.Atan2(target.position.y - transform.position.y + 1.0f, target.position.x - transform.position.x);
            }
        }
        if (startAttack)
        {
            lastAttack = Time.fixedTime; // reset attack interval
            rb.velocity = new Vector3(Mathf.Cos(attackAngle) * attackSpeed, Mathf.Sin(attackAngle) * attackSpeed, 0.0f);
            inAttack = true;
        }
        // Lock z position
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
        if (inPreAttack || inAttack)
        {
            // Point in direction of travel
            transform.right = rb.velocity.normalized;
        }
        else
        {
            // Only rotate on one axis
            rb.angularVelocity = new Vector3(0.0f, 0.0f, rb.angularVelocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        inAttack = false;
    }
}
