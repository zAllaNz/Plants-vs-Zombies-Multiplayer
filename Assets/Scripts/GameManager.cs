using System.Collections;
using UnityEngine;
using TMPro;



public class GameManager : MonoBehaviour
   


{ 
    public static GameManager instance;  

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

    // para manter as animaçãoes 
    public GameObject zombiePreviewObject;

  


    void Start()
    {
        currentSun = 250;
        currentBrains = 1000; // Exemplo de valor inicial
        UpdateBrainsText();
        //StartCoroutine(SpawnSunRoutine());
        StartCoroutine(SpawnBrainRoutine());
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
        if (brainsText != null) // verifica se o texto foi atribuído
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
