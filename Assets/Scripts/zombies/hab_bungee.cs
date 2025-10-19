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

    [Header("Controle de Mira Bungee")]
    public bool modoMiraBungeeAtivo = false; // Flag que controla o modo de mira
    private GameObject prefabBungeeParaInvocar;
    private int custoBungeeParaGastar;

    // Esta função será chamada pelo BOTÃO
    public void IniciarModoMiraBungee(GameObject prefabBungee, int custo)
    {
        if (modoMiraBungeeAtivo)
        {
            CancelarMira();
            return;
        }

        // Pergunta ao GameManager se tem cérebros suficientes para efetuar a ação
        if (GameManager.instance.currentBrains < custo)
        {
            Debug.Log("Cérebros insuficientes para INICIAR a mira.");
            return;
        }

        // Ativa o modo de mira
        modoMiraBungeeAtivo = true;
        prefabBungeeParaInvocar = prefabBungee;
        custoBungeeParaGastar = custo;
        Debug.Log("Modo de mira Bungee ATIVADO. Clique em uma planta.");


    }
    public  void ConfirmarAlvoBungee(GameObject plantaAlvo)
    {
        if (!modoMiraBungeeAtivo) return;

        // 3. Tenta gastar os cérebros (verificação final)
        // Pede ao GameManager para fazer a transação
        if (GameManager.instance.SpendBrains(custoBungeeParaGastar))
        {
            // 4. Sucesso! Invoca o zumbi
            Debug.Log("Alvo " + plantaAlvo.name + " confirmado. Enviando zumbi.");

            GameObject bungeeInvocado = Instantiate(prefabBungeeParaInvocar, Vector3.zero, Quaternion.identity);
            bungeeInvocado.GetComponent<BungeeZombie>().plantaAlvo = plantaAlvo;
        }
        else
        {
            Debug.LogError("Falha ao gastar cérebros mesmo após checagem inicial.");
        }

        // 5. Desativa o modo de mira
        CancelarMira();
    }

    public void CancelarMira()
    {
        modoMiraBungeeAtivo = false;
        prefabBungeeParaInvocar = null;
        custoBungeeParaGastar = 0;
        Debug.Log("Modo de mira CANCELADO.");


    }
    void Update()
    {
        if (modoMiraBungeeAtivo && Input.GetMouseButtonDown(1)) // 1 = Botão Direito
        {
            CancelarMira();
        }
    }
}
