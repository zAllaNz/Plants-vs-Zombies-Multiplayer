using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject current_zombi;
    public GameObject current_plant;
    public Sprite current_plant_sprite;
    public Transform tiles;
    public LayerMask tile_layer;
    public int currentSun;

    public GameObject sunPrefab;
    public float minX; // Posi��o X m�nima para o spawn
    public float maxX; // Posi��o X m�xima para o spawn
    public float spawnY; // Posi��o Y de onde os s�is v�o cair
    public float minSpawnInterval = 5.0f;
    public float maxSpawnInterval = 12.0f;

    public Sprite current_zombie_sprite;

    public Transform tiles;

    public LayerMask tileMask;

    public void comprar_zombie(GameObject zombie, Sprite sprite)
    {
        current_zombi = zombie;
        current_zombie_sprite = sprite;
        currentSun = 250;
        StartCoroutine(SpawnSunRoutine());
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition), 
            Vector2.zero, 
            Mathf.Infinity,
            tileMask);

        foreach (Transform tile in tiles)
            tile.GetComponent<SpriteRenderer>().enabled = false;

        if(hit.collider && current_zombi)
        {
            hit.collider.GetComponent<SpriteRenderer>().sprite = current_zombie_sprite;
            hit.collider.GetComponent<SpriteRenderer>().enabled = true;

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(current_zombi, hit.collider.transform.position, Quaternion.identity);
                current_zombi = null;
                current_zombie_sprite = null;
            }

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
};