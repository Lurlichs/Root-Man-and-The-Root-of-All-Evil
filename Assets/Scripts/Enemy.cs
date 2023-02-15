using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public Player player;
    public StageController stageController;

    [SerializeField] private Rigidbody rb;
    [SerializeField] public GameObject deathEffect;
    [SerializeField] public bool isBoss;

        /// <summary>
        /// Use if necessary
        /// </summary>
    public void Setup(Player player, StageController stageController)
    {
        this.player = player;
        this.stageController = stageController;

        StartCoroutine(HeightCheck());
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        
        if(isBoss == true)
        {
            UI_Manager.Instance.UpdateBossHealth(health);
        }
       

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Please inherit this as apparently some enemies has a unique death
        stageController.RemoveDeadEnemy(gameObject);

        GameObject spawnEffect = Instantiate(deathEffect);
        spawnEffect.transform.position = transform.position;

        //AudioPlayer.Instance.PlayClip("enemyDeath", 1f, true);

        //UI_Manager.Instance.FadeOutBossBar();

        Destroy(gameObject);
    }

    public void Throw(Vector3 playerTransform)
    {
        float force = 10;
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);

        if (playerTransform.x > transform.position.x)
        {
            rb.AddForce(Vector3.left * force, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.right * force, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// If it goes out of bounds, then it dies
    /// </summary>
    /// <returns></returns>
    private IEnumerator HeightCheck()
    {
        while (true)
        {
            if(transform.position.y > 25)
            {
                Die();
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
