using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSpikeSpawn : MonoBehaviour
{
    [Tooltip("Number of root spike objects")]
    public int numRootSpikeSpawns = 8;
    [Tooltip("Initial delay before the first spawn")]
    public float initialDelay = 1.5f;
    [Tooltip("Time interval in seconds between root spike spawns")]
    public float rootSpikeTimeInterval = 0.5f;
    [Tooltip("Distance interval between root spike spawns")]
    public float rootSpikeDistance = 5.0f;
    [Tooltip("Factor to apply for distance for first spawn")]
    public float initialDistanceFactor = 1.5f;

    [Tooltip("Put root spike prefab object here")]
    public GameObject rootSpikePrefab;

    public void Spawn()
    {
        StartCoroutine("SpawnSpikes");
    }

    IEnumerator SpawnSpikes()
    {
        GameObject[] rootSpikes = new GameObject[numRootSpikeSpawns];
        yield return new WaitForSeconds(initialDelay);
        for (int i = 0; i < numRootSpikeSpawns; i++)
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
                pos = rootSpikes[i - 1].transform.position;
            }
            float offset = rootSpikeDistance;
            if (i == 0)
            {
                offset *= initialDistanceFactor;
            }
            pos += Vector3.left * offset;
            rootSpikes[i] = Instantiate(rootSpikePrefab, transform);
            rootSpikes[i].transform.position = pos;
            yield return new WaitForSeconds(rootSpikeTimeInterval);
        }
        for (int i = 0; i < numRootSpikeSpawns; i++)
        {
            Destroy(rootSpikes[i]);
            rootSpikes[i] = null;
            if (i != (numRootSpikeSpawns - 1))
            {
                yield return new WaitForSeconds(rootSpikeTimeInterval * 0.75f);
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
