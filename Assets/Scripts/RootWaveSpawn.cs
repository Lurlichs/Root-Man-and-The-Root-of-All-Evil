using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootWaveSpawn : MonoBehaviour
{
    [Tooltip("Number of root wave objects")]
    public int numRootWaveSpawns = 3;
    [Tooltip("Initial delay before the first spawn")]
    public float initialDelay = 1.5f;
    [Tooltip("Time interval in seconds between root wave spawns")]
    public float rootWaveTimeInterval = 1.0f;
    [Tooltip("Minimum distance interval between root wave spawns")]
    public float rootWaveDistanceMin = 10.0f;
    [Tooltip("Maximum distance interval between root wave spawns")]
    public float rootWaveDistanceMax = 15.0f;
    [Tooltip("Factor to apply for distance for first spawn")]
    public float initialDistanceFactor = 1.5f;
    [Tooltip("Whether this is a player root wave")]
    public bool isPlayer = false;

    [Tooltip("Put root wave prefab object here")]
    public GameObject rootWavePrefab;

    public void Spawn(bool facingLeft)
    {
        StartCoroutine("SpawnRoots", facingLeft);
    }

    IEnumerator SpawnRoots(bool facingLeft)
    {
        GameObject[] rootWaves = new GameObject[numRootWaveSpawns];
        yield return new WaitForSeconds(initialDelay);
        for (int i = 0; i < numRootWaveSpawns; i++)
        {
            Vector3 pos;
            if (i == 0)
            {
                // Spawn to left of tree
                pos = transform.position;
            }
            else
            {
                // Spawn to left of the last root
                pos = rootWaves[i - 1].transform.position;
            }
            float offset = Random.Range(rootWaveDistanceMin, rootWaveDistanceMax);
            if (i == 0)
            {
                offset *= initialDistanceFactor;
            }
            pos += Vector3.left * offset;
            rootWaves[i] = Instantiate(rootWavePrefab, transform);
            rootWaves[i].transform.position = pos;
            if (facingLeft)
            {
                rootWaves[i].transform.right = Vector3.left; // mirror right to left
            }
            if (isPlayer)
            {
                Collider[] colliders = GetComponentsInChildren<Collider>();
                foreach (Collider c in colliders)
                {
                    // Stop the collider from damaging the player
                    c.tag = "Untagged";
                }
            }
            DamagesEnemies[] enemyDamagers = GetComponentsInChildren<DamagesEnemies>();
            foreach (DamagesEnemies damager in enemyDamagers)
            {
                damager.DoesDamageEnemies = isPlayer;
            }
            //Debug.Log("Spawn root wave " + i);
            yield return new WaitForSeconds(rootWaveTimeInterval);
        }
        for (int i = numRootWaveSpawns - 1; i >= 0; i--)
        {
            Destroy(rootWaves[i]);
            //Debug.Log("Destroy root wave " + i);
            rootWaves[i] = null;
            if (i != 0)
            {
                yield return new WaitForSeconds(rootWaveTimeInterval * 0.75f);
            }
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
