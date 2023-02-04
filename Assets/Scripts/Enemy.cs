using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private Player player;

    /// <summary>
    /// Use if necessary
    /// </summary>
    public void Setup(Player player)
    {
        this.player = player;
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
