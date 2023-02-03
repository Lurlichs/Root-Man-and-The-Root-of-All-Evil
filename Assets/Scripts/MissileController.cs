using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Missile type enemy that tries to launch itself in an arc at the player
public class MissileController : MonoBehaviour
{
    public float attackFrequency = 10.0f;
    public float preAttackSpeed = 10.0f;
    public float minAngle = 20.0f;
    public float maxAngle = 60.0f;

    private float lastAttack;
    private Rigidbody rb;
    private bool inPreAttack;
    private bool inAttack;
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

    // Calculates the speed of a launch required to hit a target at requiredXDifference
    // from the launcher, when angled at angle (in degrees)
    private float CalculateLaunchSpeed(float requiredXDifference, float angle)
    {
        float angleRadian = angle * Mathf.Deg2Rad;
        float gravity = Mathf.Abs(Physics.gravity.y);
        return Mathf.Sqrt(requiredXDifference * gravity / (2 * Mathf.Cos(angleRadian) * Mathf.Sin(angleRadian)));
    }

    void FixedUpdate()
    {
        bool startAttack = false;

        // Check if time to do a new attack
        if ((Time.fixedTime - lastAttack) > attackFrequency)
        {
            lastAttack = Time.fixedTime;
            // Launch a new attack
            if (target.position.y >= (transform.position.y + 0.5f))
            {
                // Need a preattack where it launches into the air up to the height of the target
                inPreAttack = true;
            }
            else
            {
                startAttack = true;
            }
            
            // Go a bit into the air to avoid colliding with the floor
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, 0.0f);
        }
        if (inPreAttack)
        {
            rb.velocity = new Vector3(0.0f, preAttackSpeed, 0.0f);
            if (target.position.y <= transform.position.y)
            {
                // Up to required, height, start real attack
                startAttack = true;
                inPreAttack = false;
            }
        }
        if (startAttack)
        {
            lastAttack = Time.fixedTime; // reset attack interval
            float angle = Random.Range(minAngle, maxAngle);
            if (target.position.x < transform.position.x)
            {
                // Launch towards left
                angle = 180.0f - angle;
            }
            float speed = CalculateLaunchSpeed(target.position.x - transform.position.x, angle);
            rb.velocity = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * speed, Mathf.Sin(angle * Mathf.Deg2Rad) * speed, 0.0f);
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
