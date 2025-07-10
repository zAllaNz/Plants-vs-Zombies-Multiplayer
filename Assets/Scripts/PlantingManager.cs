using UnityEngine;

public class PlantingManager : MonoBehaviour
{
    public static PlantingManager Instance;

    private PlantData selectedPlantData; // A planta que o jogador selecionou
    private GameManager gameManager; // Referência para o seu GameManager

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Encontra o GameManager na cena para gerenciar os sóis
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Método chamado pelo card da UI quando é clicado
    public void SelectPlant(PlantData plantData)
    {
        // Verifica se o jogador tem sóis suficientes para selecionar
        if (gameManager.currentSun >= plantData.sunCost)
        {
            selectedPlantData = plantData;
            Debug.Log("Planta selecionada: " + selectedPlantData.plantName);
        }
        else
        {
            Debug.Log("Sóis insuficientes para selecionar " + plantData.plantName);
            selectedPlantData = null; // Garante que nada esteja selecionado
        }
    }

    // Método que o slot do grid vai chamar para obter o prefab
    public GameObject GetSelectedPlantPrefab()
    {
        if (selectedPlantData != null)
        {
            return selectedPlantData.plantPrefab;
        }
        return null;
    }

    // Método para consumir os sóis e deselecionar a planta após o plantio
    public void PlantWasPlaced()
    {
        if (selectedPlantData != null)
        {
            gameManager.SpendSun(selectedPlantData.sunCost); // Supondo que você tenha um método SpendSun no GameManager
            selectedPlantData = null; // Deseleciona a planta para que não seja plantada de novo sem querer
        }
    }
}