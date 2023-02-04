using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power
{
    public int id;
    public string name;
    public bool currentlyActive;

    public Power(int id, string name, bool active)
    {
        this.id = id;
        this.name = name;
        this.currentlyActive = active;
    }
}

public enum PlayerState
{
    Idle,
    Walking,
    Jumping,
    Falling,
    Shielding,
    Hurt,
    RootWave,
    ShootingIdle,
    ShootingWalking,
    ShootingJumping,
    ShootingFalling
}

public class Player : MonoBehaviour
{
    [Header("Modifyable Constants")]
    [SerializeField] private int maxHealth;

    [SerializeField] private float speed;
    [SerializeField] private float airDrag;

    // Recoil refers to the animation that plays when player gets hurt
    [SerializeField] private float recoilTime;
    [SerializeField] private float recoilPowerX;
    [SerializeField] private float recoilPowerY;
    // Refers to how long players get invulnerability when taking damage
    [SerializeField] private float invulnerabilityTime;

    [SerializeField] private float rootWaveCooldown;
    [SerializeField] private float regenerationCooldown;
    [SerializeField] private float projectileCooldown;

    [SerializeField] private float playerHeight;

    [SerializeField] private float attackDamage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float meleeDistance;
    // How long does the player keep up the "Shooting" state if they are shooting
    [SerializeField] private float attackStateDuration;

    [SerializeField] private float jumpPower;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float gravityMultiplierFalling;

    [Header("Object")]
    [SerializeField] private GameObject player;
    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private GameObject leaf;

    [Header("Condition")]
    public bool alive;
    public bool playing;

    [Header("Powers")]
    private int powersCount;
    [SerializeField]
    private List<Power> powers = new List<Power>
    {
        new Power(0, "regen", true),
        new Power(1, "projectile", true),
        new Power(2, "doubleJump", true),
        new Power(3, "rootWave", true),
        new Power(4, "shield", true),
    };

    [Header("Prefabs")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject meleeHitPrefab;
    [SerializeField] private GameObject regenParticles;
    [SerializeField] private GameObject jumpParticles;
    [SerializeField] private GameObject punchParticles;
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private GameObject shieldReleasePrefab;

    [Header("In game states")]
    public PlayerState playerState;

    public int currentHealth;
    [SerializeField] private bool facingLeft;

    [SerializeField] private float currentSpeed;

    [SerializeField] private float currentAttackCooldown;
    [SerializeField] private float currentProjectileStateDuration;
    public float currentRecoilTime;
    [SerializeField] private float currentInvulnerabilityTime;

    [SerializeField] private float currentRootWaveCooldown;
    [SerializeField] private float currentRegenerationCooldown;

    [SerializeField] private LayerMask ground;
    [SerializeField] private bool grounded;

    [SerializeField] private bool doubleJumpAvailable;
    public bool activatingShield;

    [SerializeField] private GameObject currentShieldObject;

    public bool pressingWalk;

    private Animator animator;


    /*[Header("Behind the scenes stuff")]
    [SerializeField] private List<Projectile> projectiles;*/

    private void Start()
    {
        Setup();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Ticks invulnerabilities and other cooldowns
    private void Update()
    {
        float time = Time.deltaTime;

        if (currentInvulnerabilityTime > 0)
        {
            currentInvulnerabilityTime -= time;

            if (currentInvulnerabilityTime <= 0)
            {
                currentInvulnerabilityTime = 0;
            }
        }

        if (currentRecoilTime > 0)
        {
            currentRecoilTime -= time;
        }

        if (currentRecoilTime <= 0)
        {
            currentRecoilTime = 0;
        }

        if (currentRootWaveCooldown > 0 && GetPowerByName("rootWave").currentlyActive)
        {
            currentRootWaveCooldown -= time;
        }

        if (currentRootWaveCooldown <= 0)
        {
            currentRootWaveCooldown = 0;
        }

        if (currentRegenerationCooldown > 0 && GetPowerByName("regen").currentlyActive)
        {
            currentRegenerationCooldown -= time;
        }

        if (currentRegenerationCooldown <= 0)
        {
            currentRegenerationCooldown = 0;
            RegenHealth();
        }

        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= time;

            if (currentAttackCooldown <= 0)
            {
                currentAttackCooldown = 0;
            }
        }

        if (currentProjectileStateDuration > 0)
        {
            currentProjectileStateDuration -= time;

            if (currentProjectileStateDuration <= 0)
            {
                currentProjectileStateDuration = 0;
            }
        }

        if (Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.15f, ground))
        {
            grounded = true;
            doubleJumpAvailable = true;
            currentSpeed = speed;
        }
        else
        {
            grounded = false;
            currentSpeed = speed / airDrag;
        }

        if (rb.velocity.y < -0.1f && currentRecoilTime == 0)
        {
            if (currentProjectileStateDuration > 0)
            {
                playerState = PlayerState.ShootingFalling;
            }
            else
            {
                playerState = PlayerState.Falling;
            }

            rb.velocity += (gravityMultiplier + gravityMultiplierFalling - 1) * Physics.gravity.y * time * Vector3.up;
        }
        else if (rb.velocity.y > 0 && currentRecoilTime == 0)
        {
            if (currentProjectileStateDuration > 0)
            {
                playerState = PlayerState.ShootingJumping;
            }
            else
            {
                playerState = PlayerState.Jumping;
            }

            rb.velocity += (gravityMultiplier - 1) * Physics.gravity.y * time * Vector3.up;
        }
        else if (rb.velocity == Vector3.zero && !activatingShield && currentRecoilTime == 0 && !pressingWalk)
        {
            if (currentProjectileStateDuration > 0)
            {
                playerState = PlayerState.ShootingIdle;
            }
            else
            {
                playerState = PlayerState.Idle;
            }
        }

       

        if (pressingWalk == false)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Run", false);
        }

        if (pressingWalk == true || grounded == false)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Run", true);
        }
        if (playerState == PlayerState.ShootingIdle)
        {
            animator.SetBool("Attack", true);
        }

    }

    public void Setup()
    {
        alive = true;
        playing = true;
        facingLeft = true;

        foreach (Power power in powers)
        {
            power.currentlyActive = true;
        }

        //GetPowerByName("projectile").currentlyActive = false;

        currentHealth = maxHealth;
        rb.velocity = new Vector3();

        currentAttackCooldown = 0;
        currentProjectileStateDuration = 0;
        currentRecoilTime = 0;
        currentInvulnerabilityTime = 0;
        currentRootWaveCooldown = 0;
        currentRegenerationCooldown = 0;
        doubleJumpAvailable = true;
    }

    public void MoveDirection(bool left, float deltaTime)
    {
        if (left)
        {
            facingLeft = true;
            transform.position = new Vector3(transform.position.x - currentSpeed * deltaTime, transform.position.y);
        }
        else
        {
            facingLeft = false;
            transform.position = new Vector3(transform.position.x + currentSpeed * deltaTime, transform.position.y);
        }

        if (grounded)
        {
            if (currentProjectileStateDuration > 0)
            {
                playerState = PlayerState.ShootingWalking;
            }
            else
            {
                playerState = PlayerState.Walking;
            }
        }
    }

    public void ActivateShield()
    {
        activatingShield = true;
        playerState = PlayerState.Shielding;

        if(currentShieldObject == null)
        {
            currentShieldObject = Instantiate(shieldPrefab);
            currentShieldObject.transform.position = transform.position;
            currentShieldObject.transform.SetParent(transform);
        }
    }

    public void DeactivateShield()
    {
        activatingShield = false;

        Destroy(currentShieldObject);

        GameObject particles = Instantiate(shieldReleasePrefab);
        particles.transform.position = transform.position;
    }

    /// <summary>
    /// TRIES to shoot a projectile if enabled, else will do a melee.
    /// </summary>
    public void Attack()
    {
        if (currentAttackCooldown == 0)
        {
            // Ranged attack
            if (GetPowerByName("projectile").currentlyActive)
            {
                currentProjectileStateDuration = attackStateDuration;
                currentAttackCooldown = projectileCooldown;

                GameObject projectile = Instantiate(projectilePrefab);
                Projectile projectileComponent = projectile.GetComponent<Projectile>();

                //projectiles.Add(projectileComponent);
                projectileComponent.Setup(attackDamage, projectileSpeed, facingLeft);

                projectile.transform.position = transform.position;
            }
            else // Melee
            {
                currentProjectileStateDuration = attackStateDuration;
                currentAttackCooldown = projectileCooldown;

                GameObject melee = Instantiate(meleeHitPrefab);
                MeleeHit meleeHit = melee.GetComponent<MeleeHit>();

                meleeHit.Setup(attackDamage, facingLeft);

                meleeHit.transform.position = transform.position;
                if(facingLeft) {
                    meleeHit.transform.position = new Vector3(meleeHit.transform.position.x - meleeDistance, 
                        meleeHit.transform.position.y, meleeHit.transform.position.z);
                 } 
                else {
                    meleeHit.transform.position = new Vector3(meleeHit.transform.position.x + meleeDistance,
                        meleeHit.transform.position.y, meleeHit.transform.position.z);
                }
            }

        }
    }

    public void Jump()
    {
        if (grounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        else if (doubleJumpAvailable && GetPowerByName("doubleJump").currentlyActive)
        {
            rb.velocity = new Vector3();
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            doubleJumpAvailable = false;

            GameObject cloud = Instantiate(jumpParticles);
            cloud.transform.position = transform.position;
        }
        else
        {
            // Don't jump
        }
    }

    public void Die()
    {
        UI_Manager.Instance.TurnOnDeathScreen();
    }

    public void RegenHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            currentRegenerationCooldown = regenerationCooldown;
            UI_Manager.Instance.UpdateHealthSetup();
        }
    }

    /// <summary>
    /// When players take damage, they will receive knockback and enter invulnerability state
    /// </summary>
    /// <param name="source">Position of the damage source, in order to calculate the knockback</param>
    public void TryTakeDamage(Vector3 source)
    {
        if (currentInvulnerabilityTime == 0 && !activatingShield)
        {
            currentHealth--;
            UI_Manager.Instance.UpdateHealthSetup();

            if (currentHealth <= 0)
            {
                Die();
                return;
            }

            // Handles recoil
            rb.velocity = new Vector3();
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
            rb.AddForce(Vector3.up * recoilPowerY, ForceMode.Impulse);

            if (source.x - transform.position.x > 0)
            {
                // Right
                rb.AddForce(Vector3.left * recoilPowerX, ForceMode.Impulse);
            }
            else
            {
                // Left
                rb.AddForce(Vector3.right * recoilPowerX, ForceMode.Impulse);
            }

            currentInvulnerabilityTime = invulnerabilityTime;

            playerState = PlayerState.Hurt;
            currentRecoilTime = recoilTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TryTakeDamage(collision.gameObject.transform.position);
            collision.gameObject.GetComponent<Enemy>().Throw(transform.position);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Damaging"))
        {
            TryTakeDamage(collision.gameObject.transform.position);
        }
    }

    public void DisablePower(int id)
    {
        GetPowerById(id).currentlyActive = false;
    }

    public List<Power> GetRandomAvailablePower(int count)
    {
        if (powersCount > 0)
        {
            List<int> powersInt = new List<int> { 0, 1, 2, 3, 4 };

            // Remove inactive powers
            foreach (Power power in powers)
            {
                if (!power.currentlyActive)
                {
                    powersInt.Remove(power.id);
                }
            }

            if (powersInt.Count > 0)
            {
                for (int i = 0; i < powersInt.Count * 3; i++)
                {
                    int temp = powersInt[i];
                    int randomIndex = Random.Range(i, powersInt.Count);
                    powersInt[i] = powersInt[randomIndex];
                    powersInt[randomIndex] = temp;
                }

                List<Power> newPowers = new List<Power>();
                int currentCount = 0;

                foreach (int id in powersInt)
                {
                    if (currentCount < count)
                    {
                        newPowers.Add(GetPowerById(id));
                        currentCount++;
                    }
                    else { break; }
                }

                return newPowers;
            }
        }

        return null;
    }

    public Power GetPowerById(int id)
    {
        foreach (Power power in powers)
        {
            if (power.id == id)
            {
                return power;
            }
        }

        return new Power(-1, "", false);
    }

    public Power GetPowerByName(string name)
    {
        foreach (Power power in powers)
        {
            if (power.name == name)
            {
                return power;
            }
        }

        return new Power(-1, "", false);
    }

    /*
    public void RemoveProjectile(Projectile projectile)
    {
        if (projectiles.Contains(projectile))
        {
            projectiles.Remove(projectile);
        }
    }*/

    /*
    public List<Projectile> GetProjectiles()
    {
        return projectiles;
    }*/
}
