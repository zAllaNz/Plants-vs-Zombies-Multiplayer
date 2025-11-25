
using UnityEngine;
using System.Collections;


public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance;

    private ZombieData selectedZombieData;
    private GameManager gameManager;

    // Variável para guardar qual botão foi clicado
    private zombie_slot lastClickedButton;

    public LayerMask tileMask;
    public Transform tiles;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // Apenas pegue o GameManager.instance
        gameManager = GameManager.instance;
        if (gameManager == null)
        {
            Debug.LogError("ERRO: ZombieManager não encontrou o GameManager.instance!");
        }
    }

    // --- RECEBE OS DOIS ARGUMENTOS ---
    public void SelectZombie(ZombieData data, zombie_slot buttonScript)
    {
        // O botão JÁ CHECOU os cérebros, então não precisamos checar aqui
        selectedZombieData = data;
        lastClickedButton = buttonScript; // Guarda qual botão foi clicado

        Debug.Log("ZombieManager: " + data.zombieName + " selecionado.");
    }

    void Update()
    {
        foreach (Transform tile in tiles)
        {
            SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = false;
            }
        }

        if (selectedZombieData != null)
        {
            // lança o raio 
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);
            if (hit.collider != null)
            {
                // Se o mouse estiver em cima de um zumbi (que não tem a tag Tile), ele ignora.
                if (hit.collider.CompareTag("Tile"))
                {
                    // Pega o sprite do preview
                    Sprite previewSprite = selectedZombieData.icon;

                    // Pega o renderizador do CHÃO (Tile)
                    SpriteRenderer tileRenderer = hit.collider.GetComponent<SpriteRenderer>();

                    if (tileRenderer != null)
                    {
                        tileRenderer.sprite = previewSprite;
                        tileRenderer.enabled = true; // Mostra o preview
                    }

                    // Se clicar, planta o zumbi
                    if (Input.GetMouseButtonDown(0))
                    {
                        Instantiate(selectedZombieData.zombiePrefab, hit.collider.transform.position, Quaternion.identity);
                        ZombieWasPlaced();
                    }
                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                DeselectZombie();
            }


        }
    }

    // Método para confirmar o posicionamento
    public void ZombieWasPlaced()
    {
        if (selectedZombieData != null)
        {
            // 1. Gasta a moeda
            gameManager.SpendBrains(selectedZombieData.brainCost);

            // 2. --- AQUI ESTÁ A LÓGICA FALTANTE ---
            // AVISA O BOTÃO para iniciar seu cooldown
            if (lastClickedButton != null)
            {
                lastClickedButton.IniciarCooldown();
            }
            // --- FIM DA LÓGICA FALTANTE ---

            // 3. Limpa a seleção
            Debug.Log("ZombieManager: " + selectedZombieData.zombieName + " foi colocado.");
            DeselectZombie();
        }
    }

    // Função para limpar a seleção
    private void DeselectZombie()
    {
        selectedZombieData = null;
        lastClickedButton = null;
    }

    // (Os métodos GetSelected... que você tinha são desnecessários se o 
    // ZombieData tiver a função GetPreviewSprite(), mas pode mantê-los se quiser)
}