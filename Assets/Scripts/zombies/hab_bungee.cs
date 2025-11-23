using System.Collections;
using UnityEngine;

public class hab_bungee : MonoBehaviour

{
    // padrão de responsabilidade unica
    public static hab_bungee instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // padrão de responsabilidade unica 

    [Header("Configuração do Bungee")]
    public GameObject prefabBungeeParaInvocar; 
    public int custoBungeeParaGastar = 150;

    [Header("Controle de Cooldown")]
    public float tempoDeCooldown = 60.0f; 
    private bool estaEmCooldown = false;
    public UnityEngine.UI.Slider sliderCooldown;

    public UnityEngine.UI.Button botaoBungee;

    [Header("Controle de Mira Bungee")]
    public bool modoMiraBungeeAtivo = false;
    // Não precisamos mais das variáveis privadas aqui

    // Esta função será chamada pelo BOTÃO
    // Removemos os parâmetros (prefabBungee, custo)

    void Start()
    {
        // Configura o slider no início e o esconde
        if (sliderCooldown != null)
        {
            sliderCooldown.maxValue = tempoDeCooldown;
            sliderCooldown.value = 0; // Começa vazio
            sliderCooldown.gameObject.SetActive(false);
        }
    }

    public void AtivarModoMiraPeloBotao()
    {

        if (estaEmCooldown)
        {
            Debug.Log("Bungee ainda está em cooldown!");
            return; 
        }

        if (modoMiraBungeeAtivo)
        {
            CancelarMira();
            return;
        }

        // Pergunta ao GameManager se tem cérebros suficientes
        if (GameManager.instance.currentBrains <= custoBungeeParaGastar)
        {
            Debug.Log("Cérebros insuficientes para INICIAR a mira.");
            return;
        }

        // Ativa o modo de mira
        modoMiraBungeeAtivo = true;
        Debug.Log("Modo de mira Bungee ATIVADO. Clique em uma planta.");


    }

    public void ConfirmarAlvoBungee(GameObject plantaAlvo)
    {
        if (!modoMiraBungeeAtivo) return;

        // Tenta gastar os cérebros (verificação final)
      
        if (GameManager.instance.SpendBrains(custoBungeeParaGastar))
        {
            StartCoroutine(IniciarRotinaCooldown());
            // Sucesso! Invoca o zumbi
            Debug.Log("Alvo " + plantaAlvo.name + " confirmado. Enviando zumbi.");

            // Criamos o zumbi
            GameObject bungeeInvocado = Instantiate(prefabBungeeParaInvocar, Vector3.zero, Quaternion.identity);

            // avisamos script do zumbi qual é o alvo dele
            BungeeZombie scriptDoZumbi = bungeeInvocado.GetComponent<BungeeZombie>();
            if (scriptDoZumbi != null)
            {
                scriptDoZumbi.plantaAlvo = plantaAlvo;
            }
            else
            {
                Debug.LogError("O prefab do Bungee Zombie não tem o script BungeeZombie.cs!");
            }
        }
        else
        {
            Debug.LogError("Falha ao gastar cérebros (acabou no último segundo?)");
        }

        //  Desativa o modo de mira
        CancelarMira();
    }


    public void CancelarMira()
    {
        modoMiraBungeeAtivo = false;
        Debug.Log("Modo de mira CANCELADO.");
    }


    void Update()
    {
        if (modoMiraBungeeAtivo && Input.GetMouseButtonDown(1)) // 1 = Botão Direito
        {
            CancelarMira();
        }
    }


    private IEnumerator IniciarRotinaCooldown()
    {
        // estamos em cooldown e desativa o botão 
        estaEmCooldown = true;
        if (botaoBungee != null)
        {
            botaoBungee.interactable = false;
        }

        // Logica do Slider
        float timer = tempoDeCooldown; // Nosso contador regressivo

        if (sliderCooldown != null)
        {
            sliderCooldown.maxValue = tempoDeCooldown; // Define o máximo (60)
            sliderCooldown.value = tempoDeCooldown;    // Define o valor atual
            sliderCooldown.gameObject.SetActive(true); // MOSTRA o slider
        }

        Debug.Log("Bungee entrou em cooldown por " + timer + " segundos.");

        // Roda enquanto o timer for maior que zero
        while (timer > 0)
        {
            // Reduz o timer pelo tempo que passou desde o último frame
            timer -= Time.deltaTime;

            // Atualiza o visual do slider
            if (sliderCooldown != null)
            {
                sliderCooldown.value = timer;
            }

            // Espera até o próximo frame
            yield return null;
        }


        // Acabou o tempo, REATIVA TUDO
        estaEmCooldown = false; 
        if (botaoBungee != null)
        {
            botaoBungee.interactable = true; 
        }



        // Esconde o slider novamente
        if (sliderCooldown != null)
        {
            sliderCooldown.gameObject.SetActive(false);
        }

        Debug.Log("Bungee está PRONTO para usar novamente!");
    }





}
