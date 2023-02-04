using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHit : MonoBehaviour
{
    private Player player;
    private float damage;
    private bool left;

    private float age = 3f;

    public void Setup(float damage, bool left)
    {
        this.damage = damage;
        this.left = left;
    }

    void Update()
    {
        age -= Time.deltaTime;

        if (age <= 0)
        {
            Die();
        }
    }

    private void HitTarget(GameObject enemy)
    {
        enemy.GetComponent<Enemy>().TakeDamage(damage);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HitTarget(other.gameObject);
        }
    }
}
