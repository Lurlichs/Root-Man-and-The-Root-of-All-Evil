using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerController : Enemy
{
    public float acceleration = 0.5f;
    public float maximumSpeed = 2.0f;

    private bool isPositive;
    private float currentVelocity;
    private Rigidbody rb;

    [Header("Leave Blank if not a boss enemy")]
    [SerializeField] private Bosses_ScriptableObj BossScObj;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Half go right, half go left
        isPositive = Random.Range(0, 2) < 1;

        if (BossScObj != null)
        {
            //health = BossScObj.baseHealth;
            UI_Manager.Instance.SetBossHealthBar(BossScObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        // Accelerate up to maximum speed in the chosen direction
        float velocityChange = acceleration * Time.fixedDeltaTime;
        if (Mathf.Abs(currentVelocity) < maximumSpeed)
        {
            if (isPositive)
            {
                currentVelocity += velocityChange;
            }
            else
            {
                currentVelocity -= velocityChange;
            }
        }
        rb.velocity = new Vector3(currentVelocity, rb.velocity.y, rb.velocity.z);
        rb.angularVelocity = new Vector3(0.0f, 0.0f, -currentVelocity);
        // Lock z position
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if is a collision with floor
        bool isFloor = true;
        foreach (ContactPoint contact in collision.contacts)
        {
            // If contact normal is not close to being "up", it's not the floor
            float howCloseToUp = Vector3.Dot(contact.normal, Vector3.up);
            if (howCloseToUp < 0.95f)
            {
                isFloor = false;
            }    
        }
        if (!isFloor)
        {
            currentVelocity = 0.0f;
            isPositive = !isPositive;
        }
    }
}
