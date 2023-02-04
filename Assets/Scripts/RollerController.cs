using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerController : Enemy
{
    public float acceleration = 0.5f;
    public float maximumSpeed = 2.0f;
    [Tooltip("Whether the roller starts going right")]
    public bool startRight = true;

    private bool isPositive;
    private float currentVelocity;
    private Rigidbody rb;
    private LookAhead lookahead;

    [Header("Leave Blank if not a boss enemy")]
    [SerializeField] private Bosses_ScriptableObj BossScObj;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lookahead = GetComponent<LookAhead>();
        // Half go right, half go left
        isPositive = startRight;

        if (BossScObj != null)
        {
            health = BossScObj.baseHealth;
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
        if (!lookahead.CheckIfCanMoveInFront(transform, isPositive))
        {
            // Change direction
            currentVelocity = 0.0f;
            isPositive = !isPositive;
        }
    }
}
