using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject current_plant;
    public Sprite current_plant_sprite;
    public Transform tiles;
    public LayerMask tile_layer;
    public int currentSun;

    public GameObject sunPrefab;
    public float minX; // Posição X mínima para o spawn
    public float maxX; // Posição X máxima para o spawn
    public float spawnY; // Posição Y de onde os sóis vão cair
    public float minSpawnInterval = 5.0f;
    public float maxSpawnInterval = 12.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSun = 250;
        StartCoroutine(SpawnSunRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddSun(int sunValue)
    {
        currentSun += sunValue;
    }

    public void SpendSun(int sunCost)
    {
        currentSun -= sunCost;
    }

    IEnumerator SpawnSunRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            SpawnSun();
        }
    }

    void SpawnSun()
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, spawnY);
        GameObject sunObject = Instantiate(sunPrefab, spawnPosition, Quaternion.identity);
        sunScript sunScript = sunObject.GetComponent<sunScript>();

        if (sunScript != null)
        {
            sunScript.Initialize(doFall: true);
        }
    }
}
