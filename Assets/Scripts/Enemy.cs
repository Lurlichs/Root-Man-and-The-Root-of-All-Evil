using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;

    /// <summary>
    /// Use if necessary
    /// </summary>
    public void Setup(float health)
    {
        this.health = health;
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
        Destroy(gameObject);
    }
}
