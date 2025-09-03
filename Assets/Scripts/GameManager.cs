using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
   


{   // texto brains 

    public TextMeshProUGUI brainsText;

    // Moedas e referências da cena
    public int currentSun;
    public int currentBrains;
    public Transform tiles;
    public LayerMask tileMask;

   
    public GameObject sunPrefab;
    public GameObject brainPrefab;
    public float minX, maxX, spawnY;
    public float minSpawnInterval = 5.0f, maxSpawnInterval = 12.0f;


    void Start()
    {
        currentSun = 250;
        currentBrains = 1000; // Exemplo de valor inicial
        UpdateBrainsText();
        StartCoroutine(SpawnSunRoutine());
        //StartCoroutine(SpawnBrainRoutine());
    }

    void Update()
    {
        /*
        GameObject prefabToPlace = ZombieManager.Instance.GetSelectedZombiePrefab();

        // Esconde todos os previews por padrão
        foreach (Transform tile in tiles)
            tile.GetComponent<SpriteRenderer>().enabled = false;

        // (não é nulo), entra no modo de posicionamento
        if (prefabToPlace != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);

            if (hit.collider)
            {
                // Pega o sprite para o preview do ZombieManager
                Sprite previewSprite = ZombieManager.Instance.GetSelectedZombieSpriteForPreview();

                // Mostra o preview
                SpriteRenderer tileRenderer = hit.collider.GetComponent<SpriteRenderer>();
                tileRenderer.sprite = previewSprite;
                tileRenderer.enabled = true;

                // Se o mouse for clicado
                if (Input.GetMouseButtonDown(0))
                {
                    //  Cria o zumbi usando o prefab que o ZombieManager forneceu
                    Instantiate(prefabToPlace, hit.collider.transform.position, Quaternion.identity);

                    // O ZombieManager será responsável por gastar os cérebros e limpar a seleção.
                    ZombieManager.Instance.ZombieWasPlaced();
                }
            }
        }
        */
    }

    // Método para gastar os cérebros
    public void SpendBrains(int amount)
    {
        currentBrains -= amount;
        UpdateBrainsText();
        Debug.Log("GameManager: Gastou " + amount + " cérebros. Total agora: " + currentBrains);
    }

    // Função para atualizar o texto na tela
    void UpdateBrainsText()
    {
        if (brainsText != null) // Boa prática: verifica se o texto foi atribuído
        {
            brainsText.text = currentBrains.ToString();
        }
    }


    public void AddSun(int sunValue)
    {
        currentSun += sunValue;
        

    }

    public void Addbrains(int brainValor) 
    {
        currentBrains += brainValor;
        UpdateBrainsText();
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

    IEnumerator SpawnBrainRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            SpawnBrains();
            UpdateBrainsText();
        } 
    }

    void SpawnBrains()
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, spawnY);
        GameObject brainObject = Instantiate(brainPrefab, spawnPosition, Quaternion.identity);
        brainScript brainScript = brainObject.GetComponent<brainScript>();
        Debug.Log("Um cérebro apareceu!");

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
