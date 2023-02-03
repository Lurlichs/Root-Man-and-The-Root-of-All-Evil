using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Player player;
    private float damage;
    private float speed;
    private bool left;

    private float age = 5;

    public void Setup(float damage, float speed, bool left)
    {
        this.damage = damage;
        this.speed = speed;
        this.left = left;
    }

    // Update is called once per frame
    void Update()
    {
        if (left)
        {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y);
        }

        age -= Time.deltaTime;

        if(age <= 0)
        {
            Die();
        }
    }

    private void HitTarget(GameObject enemy)
    {

    }

    private void Die()
    {
        //player.RemoveProjectile(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            Die();
        }
        else if (other.CompareTag("Enemy"))
        {
            HitTarget(other.gameObject);
        }
    }
}
