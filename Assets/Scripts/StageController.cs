using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Wave
{
    public List<Spawn> spawns;
}

[System.Serializable]
public struct Spawn
{
    public GameObject enemy;
    public Vector3 spawnPos;
}

public class StageController : MonoBehaviour
{
    [Header("Constants")]
    [SerializeField] public float intermissionDuration;

    [Header("Stage info")]
    public int stageNumber;
    [SerializeField] private List<Wave> waves;

    [Header("During Gameplay")]
    public bool gameStarted;
    
    [SerializeField] private int currentWave;
    [SerializeField] private List<GameObject> livingEnemies;

    private void Update()
    {
        if (gameStarted && livingEnemies.Count == 0)
        {
            StartCoroutine(Intermission());
        }
    }

    public void TriggerNextWave()
    {
        currentWave++;

        if(currentWave <= waves.Count)
        {
            foreach (Spawn spawn in waves[currentWave].spawns)
            {
                GameObject spawnedEnemy = Instantiate(spawn.enemy);
                spawnedEnemy.transform.position = spawn.spawnPos;
                livingEnemies.Add(spawnedEnemy);
            }
        }
    }

    /// <summary>
    /// Gives a short break before starting the next wave
    /// </summary>
    /// <returns></returns>
    private IEnumerator Intermission()
    {
        yield return new WaitForSeconds(intermissionDuration);

        TriggerNextWave();
    }

    public void SpawnBoss()
    {

    }

    public void TriggerGameEnd()
    {

    }
}
