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

public class Player : MonoBehaviour
{
    [Header("Modifyable Constants")]
    [SerializeField] private int maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private float groundDrag;

    [SerializeField] private float invulnerabilityTime;
    [SerializeField] private float rootWaveCooldown;
    [SerializeField] private float regenerationCooldown;
    [SerializeField] private float projectileCooldown;

    [SerializeField] private float playerHeight;

    [SerializeField] private float projectileDamage;
    [SerializeField] private float projectileSpeed;

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
    [SerializeField] private List<Power> powers = new List<Power>
    {
        new Power(0, "regen", true),
        new Power(1, "projectile", true),
        new Power(2, "doubleJump", true),
        new Power(3, "rootWave", true),
        new Power(4, "shield", true),
    };

    [Header("Prefabs")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject regenParticles;
    [SerializeField] private GameObject jumpParticles;
    [SerializeField] private GameObject punchParticles;

    [Header("In game states")]
    [SerializeField] private int currentHealth;
    [SerializeField] private bool facingLeft;

    [SerializeField] private float currentProjectileCooldown;
    [SerializeField] private float currentInvulnerabilityTime;
    [SerializeField] private float currentRootWaveCooldown;
    [SerializeField] private float currentRegenerationCooldown;

    [SerializeField] private LayerMask ground;
    [SerializeField] private bool grounded;


    /*[Header("Behind the scenes stuff")]
    [SerializeField] private List<Projectile> projectiles;*/

    private void Start()
    {
        Setup();
    }

    // Ticks invulnerabilities and other cooldowns
    private void Update()
    {
        float time = Time.deltaTime;

        if(currentInvulnerabilityTime > 0)
        {
            currentInvulnerabilityTime -= time;

            if(currentInvulnerabilityTime <= 0)
            {
                currentInvulnerabilityTime = 0;
            }
        }

        if (currentRootWaveCooldown > 0)
        {
            currentRootWaveCooldown -= time;

            if (currentRootWaveCooldown <= 0)
            {
                currentRootWaveCooldown = 0;
            }
        }

        if (currentRegenerationCooldown > 0)
        {
            currentRegenerationCooldown -= time;

            if (currentRegenerationCooldown <= 0)
            {
                currentRegenerationCooldown = 0;
            }
        }

        if (currentProjectileCooldown > 0)
        {
            currentProjectileCooldown -= time;

            if (currentProjectileCooldown <= 0)
            {
                currentProjectileCooldown = 0;
            }
        }
    }

    public void Setup()
    {
        alive = true;
        playing = true;
        facingLeft = true;

        foreach(Power power in powers)
        {
            power.currentlyActive = true;
        }

        currentHealth = maxHealth;
    }

    public void ResetVariables()
    {
        
    }

    public void MoveDirection(bool left, float deltaTime)
    {
        /*
        Vector3 direction = new Vector3(xAxis, 0, 0).normalized;

        if(direction.magnitude >= 0.1)
        {
            rb.AddForce(direction * speed, ForceMode.Force);
        }*/

        if (left)
        {
            facingLeft = true;
            transform.position = new Vector3(transform.position.x - speed * deltaTime, transform.position.y);
        }
        else
        {
            facingLeft = false;
            transform.position = new Vector3(transform.position.x + speed * deltaTime, transform.position.y);
        }

        
    }

    /// <summary>
    /// TRIES to shoot a projectile.
    /// </summary>
    public void ShootProjectile()
    {
        if(currentProjectileCooldown == 0)
        {
            currentProjectileCooldown = projectileCooldown;

            GameObject projectile = Instantiate(projectilePrefab);
            Projectile projectileComponent = projectile.GetComponent<Projectile>();

            //projectiles.Add(projectileComponent);
            projectileComponent.Setup(projectileDamage, projectileSpeed, facingLeft);

            projectile.transform.position = transform.position;
        }
    }

    public void Die()
    {

    }

    public void RegenHealth()
    {

    }

    public void TakeDamage()
    {
        currentHealth--;

        if(currentHealth <= 0)
        {
            Die();
        }

        // Else
        // TODO: Enter invulnerability state for a bit like cuphead
    }

    public void DisablePower(int id)
    {
        GetPowerById(id).currentlyActive = false;
    }

    public List<Power> GetRandomAvailablePower(int count)
    {
        if(powersCount > 0)
        {
            List<int> powersInt = new List<int> {0, 1, 2, 3, 4};

            // Remove inactive powers
            foreach(Power power in powers)
            {
                if (!power.currentlyActive)
                {
                    powersInt.Remove(power.id);
                }
            }

            if(powersInt.Count > 0)
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

                foreach(int id in powersInt)
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
        foreach(Power power in powers)
        {
            if(power.id == id)
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
