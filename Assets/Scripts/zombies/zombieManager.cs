
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
        // esconde os previews por padrão
        foreach (Transform tile in tiles)
            tile.GetComponent<SpriteRenderer>().enabled = false;

        // Se temos um zumbi selecionado (carregando no mouse)
        if (selectedZombieData != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tileMask);

            if (hit.collider)
            {
                // Pega o sprite para o preview (precisamos do prefab)
                Sprite previewSprite = selectedZombieData.icon; // Usando a função do ZombieData

                // Mostra o preview
                SpriteRenderer tileRenderer = hit.collider.GetComponent<SpriteRenderer>();
                tileRenderer.sprite = previewSprite;
                tileRenderer.enabled = true;

                // Se o jogador clicou no tile
                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(selectedZombieData.zombiePrefab, hit.collider.transform.position, Quaternion.identity);
                    ZombieWasPlaced(); // Confirma o spawn
                }
            }

            // Permite cancelar a seleção com o botão direito
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