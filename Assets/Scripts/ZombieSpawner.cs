using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Prefabs de Zumbis (fraco → forte)")]
    public GameObject[] zombiePrefabs;

    [Header("Faixas (lanes)")]
    public List<Transform> spawnLanes;

    [Header("Spawn")]
    public float spawnInterval = 3f;

    [Header("Waves")]
    public WaveData[] waves;

    private int currentWave = 0;
    private float waveTimer;
    private int aliveZombies = 0;

    void Start()
    {
        if (spawnLanes.Count == 0 || zombiePrefabs.Length == 0 || waves.Length == 0)
        {
            Debug.LogError("ZombieSpawner mal configurado!");
            return;
        }

        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (currentWave < waves.Length)
        {
            waveTimer = waves[currentWave].duration;
            Debug.Log($"Wave {currentWave + 1} iniciada!");

            while (waveTimer > 0)
            {
                if (aliveZombies < waves[currentWave].maxAliveZombies)
                {
                    SpawnZombie();
                }

                yield return new WaitForSeconds(spawnInterval);
                waveTimer -= spawnInterval;
            }

            currentWave++;
        }

        Debug.Log("Todas as waves finalizadas!");
    }

    void SpawnZombie()
    {
        Transform lane = spawnLanes[Random.Range(0, spawnLanes.Count)];
        GameObject zombiePrefab = GetWeightedZombie();

        GameObject zombie = Instantiate(
            zombiePrefab,
            lane.position,
            Quaternion.identity
        );

        aliveZombies++;

        zombie zombieScript = zombie.GetComponent<zombie>();
        if (zombieScript != null)
        {
            zombieScript.spawner = this;
        }
    }

    public void OnZombieDeath()
    {
        aliveZombies--;
    }

    GameObject GetWeightedZombie()
    {
        int totalWeight = 0;
        int[] weights = new int[zombiePrefabs.Length];

        for (int i = 0; i < zombiePrefabs.Length; i++)
        {
            weights[i] = zombiePrefabs.Length - i;
            totalWeight += weights[i];
        }

        int randomValue = Random.Range(0, totalWeight);
        int cumulative = 0;

        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (randomValue < cumulative)
                return zombiePrefabs[i];
        }

        return zombiePrefabs[0];
    }
}


// Classe auxiliar
[System.Serializable]
public class WaveData
{
    public float duration = 30f;
    public int maxAliveZombies = 5;

    public WaveData(float duration, int maxAlive)
    {
        this.duration = duration;
        this.maxAliveZombies = maxAlive;
    }
}
