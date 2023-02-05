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
    [SerializeField] public GameManager gameManager;
    [SerializeField] public float intermissionDuration;
    [SerializeField] private Player player;
    [SerializeField] private GameObject spawnParticles;

    [Header("Stage info")]
    public int stageNumber;
    [SerializeField] private List<Wave> waves;

    [Header("During Gameplay")]
    public bool waveStarted;
    
    [SerializeField] private int currentWave;
    [SerializeField] private List<GameObject> livingEnemies;

    public void Setup()
    {
        TriggerNextWave();
        waveStarted = true;
    }

    private void Update()
    {
        if (waveStarted && livingEnemies.Count == 0)
        {
            StartCoroutine(Intermission());
        }
    }

    public void TriggerNextWave()
    {
        if(currentWave < waves.Count)
        {
            Debug.Log(waves[currentWave].spawns.Count);

            foreach (Spawn spawn in waves[currentWave].spawns)
            {
                GameObject spawnedEnemy = Instantiate(spawn.enemy); 

                GameObject spawnEffect = Instantiate(spawnParticles);
                spawnEffect.transform.position = spawn.spawnPos;

                spawnedEnemy.transform.position = spawn.spawnPos;
                spawnedEnemy.GetComponent<Enemy>().Setup(player, this);
                livingEnemies.Add(spawnedEnemy);
            }
        }

        currentWave++;
        waveStarted = true;
    }

    /// <summary>
    /// Gives a short break before starting the next wave
    /// </summary>
    /// <returns></returns>
    private IEnumerator Intermission()
    {
        waveStarted = false;

        yield return new WaitForSeconds(intermissionDuration);

        if(currentWave == waves.Count)
        {
            gameManager.FinishStage();
        }
        else
        {
            TriggerNextWave();
        }

    }

    public void SpawnBoss()
    {

    }

    public void TriggerGameEnd()
    {

    }

    public void AddLivingEnemy(GameObject enemy)
    {
        livingEnemies.Add(enemy);
    }

    public void RemoveDeadEnemy(GameObject gameObject)
    {
        livingEnemies.Remove(gameObject);
    }
}
