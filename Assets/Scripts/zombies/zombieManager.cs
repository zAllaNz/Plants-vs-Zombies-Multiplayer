
using UnityEngine;
using System.Collections;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance;

    private ZombieData selectedZombieData;
    private GameManager gameManager;
    public GameObject zombiePreviewObject;

// espaço pra colocas os zombies 
    public LayerMask tileMask;
    public Transform tiles;

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


    // sistemas de alocação de zombies 
    void Update()
    {
        GameObject prefabToPlace = ZombieManager.Instance.GetSelectedZombiePrefab();

        // esconde os previews por padrão 
        foreach (Transform tile in tiles)
            tile.GetComponent<SpriteRenderer>().enabled = false;

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

                if (Input.GetMouseButtonDown(0))
                {

                    Instantiate(prefabToPlace, hit.collider.transform.position, Quaternion.identity);
                    ZombieManager.Instance.ZombieWasPlaced();

                }

            }


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