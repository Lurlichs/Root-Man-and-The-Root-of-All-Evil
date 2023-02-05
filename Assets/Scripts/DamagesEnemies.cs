using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagesEnemies : MonoBehaviour
{
    public float damage;
    public bool doesDamageEnemies;

    public bool DoesDamageEnemies { get => doesDamageEnemies; set => doesDamageEnemies = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HitTarget(GameObject enemy)
    {
        Enemy enemyObject = enemy.GetComponent<Enemy>();
        if (enemyObject != null)
        {
            enemyObject.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (doesDamageEnemies && other.CompareTag("Enemy"))
        {
            HitTarget(other.gameObject);
        }
    }
}
