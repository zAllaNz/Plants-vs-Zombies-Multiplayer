// zombie_slot.cs
using TMPro; // Não se esqueça de incluir isso para usar TextMeshPro
using UnityEngine;
using UnityEngine.UI;

public class zombie_slot : MonoBehaviour
{
    [Header("Dados do Zumbi")]
    public ZombieData zombieData; // A única conexão com os dados do jogo

    [Header("Referências da UI")]
    public Image backgroundImage;   // Para o objeto chamado "Background"
    public Image iconImage;         // Para o objeto chamado "icon"
    public TextMeshProUGUI priceText; // Para o objeto chamado "price"

    // Awake é chamado antes do Start, ideal para configurar referências
    void Awake()
    {
        // Garante que o botão chame a função OnButtonClick ao ser clicado
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    // Start é chamado uma vez quando o jogo começa
    void Start()
    {
        UpdateVisuals();
    }

    // OnValidate é uma função mágica da Unity: é chamada no editor sempre que
    // um valor é alterado no Inspector. Ótimo para ver as mudanças ao vivo!
    private void OnValidate()
    {
        // É uma boa prática verificar se os componentes existem antes de usá-los no OnValidate
        if (iconImage == null) iconImage = GetComponentInChildren<Image>();
        if (priceText == null) priceText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateVisuals();
    }

    // Esta função atualiza a aparência do botão com base no ZombieData
    private void UpdateVisuals()
    {
        if (zombieData != null)
        {
            // Ativa os componentes visuais
            iconImage.enabled = true;
            priceText.enabled = true;

            // Define o ícone e o preço
            iconImage.sprite = zombieData.icon;
            priceText.text = zombieData.brainCost.ToString();
        }
        else
        {
            // Se nenhum ZombieData for fornecido, esconde tudo
            iconImage.enabled = false;
            priceText.enabled = false;
        }
    }

    // Esta função é a lógica do jogo: o que acontece ao clicar


    void OnButtonClick()
    {
        // Adicione esta linha:
        Debug.Log("Botão CLICADO COM SUCESSO! Tentando selecionar: " + zombieData.zombieName);

        if (zombieData != null)
        {
            ZombieManager.Instance.SelectZombie(zombieData);
        }
    }
}