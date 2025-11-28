using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawner : MonoBehaviour
{
    public GameObject[] plantPrefabs;
    public List<Transform> allGridSquares;
    public float initialDelay = 1f;
    public float spawnInterval = 5f;
    void Start()
    {
        if (allGridSquares == null || allGridSquares.Count == 0)
        {
            Debug.LogError("A lista 'allGridSquares' está vazia! Atribua os squares da grid no Inspector.");
            return;
        }

        InvokeRepeating("SpawnRandomPlant", initialDelay, spawnInterval);
    }

    void SpawnRandomPlant()
    {
        if (plantPrefabs.Length == 0)
        {
            Debug.LogError("Nenhum Plant Prefab atribuído ao PlantSpawner!");
            return;
        }

        int randomSquareIndex = Random.Range(0, allGridSquares.Count);
        Transform spawnSquare = allGridSquares[randomSquareIndex];
        if (spawnSquare.childCount == 0)
        {
            int randomPlantIndex = Random.Range(0, plantPrefabs.Length);
            GameObject plantToSpawn = plantPrefabs[randomPlantIndex];
            GameObject newPlant = Instantiate(plantToSpawn, spawnSquare.position, Quaternion.identity);
            newPlant.transform.SetParent(spawnSquare);
            Debug.Log($"Planta '{plantToSpawn.name}' spawnada em: {spawnSquare.name}");
        }
        else
        {
            Debug.Log($"O square '{spawnSquare.name}' já está ocupado. Tentando novamente no próximo intervalo.");
        }
    }
}