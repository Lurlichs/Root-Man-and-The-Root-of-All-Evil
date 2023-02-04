using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public Player player;
    public StageController stageController;

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
        Destroy(gameObject);
    }
}
