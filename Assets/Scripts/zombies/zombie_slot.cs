

using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Importante para Coroutines

public class zombie_slot : MonoBehaviour
{
    [Header("Dados do Zumbi")]
    public ZombieData zombieData;

    [Header("Referências da UI")]
    public Image backgroundImage;
    public Image iconImage;
    public TextMeshProUGUI priceText;

    // --- VARIÁVEIS DE COOLDOWN  ---
    [Header("Controle de Cooldown")]
    public float tempoDeCooldown = 8.0f; // Defina o tempo de recarga aqui
    private bool estaEmCooldown = false;
    private Slider sliderCooldown;
    private Button botao; // Referência ao próprio botão
   

    void Awake()
    {
        // Pega o botão neste objeto
        botao = GetComponent<Button>();
        botao.onClick.AddListener(OnButtonClick);
    }

    void Start()
    {
       
        // Procura o slider nos objetos filhos
        sliderCooldown = GetComponentInChildren<Slider>(true); // 'true' inclui inativos
        if (sliderCooldown != null)
        {
            sliderCooldown.maxValue = tempoDeCooldown;
            sliderCooldown.value = 0;
            sliderCooldown.gameObject.SetActive(false);
        }
        

        UpdateVisuals();
    }

    private void OnValidate()
    {
        if (iconImage == null) iconImage = GetComponentInChildren<Image>();
        if (priceText == null) priceText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        
        if (zombieData != null)
        {
            iconImage.enabled = true;
            priceText.enabled = true;
            iconImage.sprite = zombieData.icon;
            priceText.text = zombieData.brainCost.ToString();
        }
        else
        {
            iconImage.enabled = false;
            priceText.enabled = false;
        }
    }

    // Função chamada pelo clique do botão
    void OnButtonClick()
    {
      
        // Checa se está em cooldown
        if (estaEmCooldown)
        {
            Debug.Log("Botão ainda em cooldown.");
            return;
        }

        // Checa se tem cérebros
        if (GameManager.instance.currentBrains < zombieData.brainCost)
        {
            Debug.Log("Cérebros insuficientes!");
            return;
        }

        // Se tudo estiver certo, seleciona o zumbi
        // (O erro de argumento será corrigido no próximo script)
        if (zombieData != null)
        {
            ZombieManager.Instance.SelectZombie(zombieData, this);
        }
        // --- FIM DA MUDANÇA ---
    }

  

    // O ZombieManager vai chamar esta função
    public void IniciarCooldown()
    {
        StartCoroutine(RotinaCooldown());
    }

    private IEnumerator RotinaCooldown()
    {
        estaEmCooldown = true;
        botao.interactable = false; // Desativa o botão

        float timer = tempoDeCooldown;

        if (sliderCooldown != null)
        {
            sliderCooldown.maxValue = tempoDeCooldown;
            sliderCooldown.value = tempoDeCooldown;
            sliderCooldown.gameObject.SetActive(true);
        }

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if (sliderCooldown != null)
            {
                sliderCooldown.value = timer;
            }
            yield return null;
        }

        // Acabou o cooldown
        estaEmCooldown = false;
        botao.interactable = true; // Reativa o botão

        if (sliderCooldown != null)
        {
            sliderCooldown.gameObject.SetActive(false);
        }
    }
}
 