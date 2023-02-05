using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns and throws a bunch of potatoes
public class PotatoSpammer : MonoBehaviour
{
    [Tooltip("Number of potatoes to spawn")]
    public int numSpawns = 12;
    [Tooltip("Initial delay before the first spawn")]
    public float initialDelay = 1.5f;
    [Tooltip("Time interval in seconds between spawns")]
    public float spawnTimeInterval = 0.1f;
    [Tooltip("Speed of potatoes being launched")]
    public float potatoSpeed = 10.0f;
    [Tooltip("Potato launch angle minimum")]
    public float minAngle = 35.0f;
    [Tooltip("Potato launch angle maximum")]
    public float maxAngle = 55.0f;
    [Tooltip("Displacement from parent position that the potatoes will spawn from")]
    public Vector3 spawnDisplacement;

    [Tooltip("Put potato prefab object here")]
    public GameObject potatoPrefab;

    public void Spawn()
    {
        StartCoroutine("SpawnPotatoes");
    }

    IEnumerator SpawnPotatoes()
    {
        yield return new WaitForSeconds(initialDelay);
        StageController sc = FindObjectOfType<StageController>();
        Player myPlayer = FindObjectOfType<Player>();
        for (int i = 0; i < numSpawns; i++)
        {
            GameObject g = Instantiate(potatoPrefab, transform);
            
            g.GetComponent<Enemy>().Setup(myPlayer, sc);
            sc.AddLivingEnemy(g);
            g.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
            g.transform.position = transform.position + spawnDisplacement;
            Debug.DrawLine(transform.position, g.transform.position);
            Rigidbody rb = g.GetComponent<Rigidbody>();
            float angle = Random.Range(minAngle, maxAngle);
            rb.velocity = new Vector3(-potatoSpeed * Mathf.Cos(angle * Mathf.Deg2Rad), potatoSpeed * Mathf.Sin(angle * Mathf.Deg2Rad), 0.0f);
            g.GetComponent<RollerController>().Fly();
            yield return new WaitForSeconds(spawnTimeInterval);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
