using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // método singleton 
    void Awake()
    {
        // Configuração do Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // Se outra instância já existe, destrói esta para garantir que só há uma
            Debug.LogWarning("Múltiplas instâncias de GameManager detectadas! Destruindo a duplicada.");
            Destroy(gameObject);
        }
    }


    public TextMeshProUGUI brainsText;

    // Moedas e referências da cena
    public int currentSun;
    public int currentBrains;
    public Transform tiles;
    public LayerMask tileMask;

    // vou testar algo
    public List<GameObject> plantasEmCampo = new List<GameObject>();


    public GameObject sunPrefab;
    public GameObject brainPrefab;
    public float minX, maxX, spawnY;
    public float minSpawnInterval = 5.0f, maxSpawnInterval = 12.0f;

    // para manter as animaçãoes 
    public GameObject zombiePreviewObject;

  


    void Start()
    {
        currentSun = 250;
        currentBrains = 300; // Exemplo de valor inicial
        UpdateBrainsText();
        //StartCoroutine(SpawnSunRoutine());
        StartCoroutine(SpawnBrainRoutine());
    }


    // Método para gastar os cérebros
    public bool SpendBrains(int amount)
    {
        // Verifica se tem cérebros suficientes
        if (currentBrains >= amount)
        {
            // Se sim, true
            currentBrains -= amount;
            UpdateBrainsText();
            Debug.Log("GameManager: Gastou " + amount + " cérebros. Total agora: " + currentBrains);
            return true;
        }
        else
        {
            // Se não, False
            Debug.Log("GameManager: Cérebros insuficientes. TENTOU gastar " + amount);
            return false;
        }
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

    // Método para adicionar uma planta à lista quando ela é criada
    public void AdicionarPlantaNaLista(GameObject planta)
    {
        if (!plantasEmCampo.Contains(planta))
        {
            plantasEmCampo.Add(planta);
            Debug.Log("GameManager: " + planta.name + " foi ADICIONADA à lista. Total: " + plantasEmCampo.Count);
        }
    }

    public void RemoverPlantaDaLista(GameObject planta)
    {
        if (plantasEmCampo.Contains(planta))
        {
            plantasEmCampo.Remove(planta);
            Debug.Log("GameManager: " + planta.name + " foi REMOVIDA da lista. Total: " + plantasEmCampo.Count);
        }
    }


}
