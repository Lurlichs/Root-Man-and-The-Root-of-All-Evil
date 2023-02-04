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

    /// <summary>
    /// Use if necessary
    /// </summary>
    public void Setup(Player player, StageController stageController)
    {
        this.player = player;
        this.stageController = stageController;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

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
}
