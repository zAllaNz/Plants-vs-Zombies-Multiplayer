
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance;

    private ZombieData selectedZombieData;
    private GameManager gameManager; 

    void Awake()
    {
        if (Instance == null) { 
            Instance = this; 
        } else {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("ERRO: ZombieManager não encontrou o GameManager na cena!");
        }
    }

    public void SelectZombie(ZombieData data)
    {
        if (gameManager.currentBrains >= data.brainCost)
        {
            selectedZombieData = data;
            Debug.Log("ZombieManager: " + data.zombieName + " selecionado.");
        }
        else
        {
            Debug.Log("ZombieManager: Cérebros insuficientes!");
            selectedZombieData = null;
        }
    }

    //  MÉTODO PARA OBTER O PREFAB 
    public GameObject GetSelectedZombiePrefab()
    {
        if (selectedZombieData != null)
        {
            return selectedZombieData.zombiePrefab;
        }
        return null;
    }

    //  MÉTODO PARA OBTER O SPRITE DO PREVIEW 
    public Sprite GetSelectedZombieSpriteForPreview()
    {
        if (selectedZombieData != null && selectedZombieData.zombiePrefab != null)
        {
            // Pega o sprite diretamente do prefab
            return selectedZombieData.zombiePrefab.GetComponent<SpriteRenderer>().sprite;
        }
        return null;
    }

    //  MÉTODO PARA CONFIRMAR O POSICIONAMENTO 
    public void ZombieWasPlaced()
    {
        if (selectedZombieData != null)
        {
            // Gasta a moeda
            gameManager.SpendBrains(selectedZombieData.brainCost);

            // Limpa a seleção
            Debug.Log("ZombieManager: " + selectedZombieData.zombieName + " foi colocado. Seleção limpa.");
            selectedZombieData = null;
        }
    }
}