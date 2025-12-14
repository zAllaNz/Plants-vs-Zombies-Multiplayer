using UnityEngine;
using System.Collections;

public class PotatoMineController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject explosionPrefab; // O efeito de explosão (que dá dano)
    public float armingTime = 14f;     // Tempo para ficar pronta (padrão é alto, diminua para testar)

    private bool isArmed = false;      // Controle de estado
    private Animator animator;
    private Collider2D myCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Tenta pegar o colisor (pode estar no pai ou filho)
        myCollider = GetComponent<Collider2D>();
        if (myCollider == null) myCollider = GetComponentInChildren<Collider2D>();

        if (myCollider != null)
        {
            // Começa INTANGÍVEL (zumbis passam por cima sem ativar)
            myCollider.enabled = false;
        }
        else
        {
            Debug.LogError("PotatoMine: Faltando BoxCollider2D!");
        }

        // Inicia contagem
        StartCoroutine(ArmRoutine());
    }

    IEnumerator ArmRoutine()
    {
        // Espera o tempo de armar
        yield return new WaitForSeconds(armingTime);

        // --- ARMOU! ---
        isArmed = true;

        // Ativa o colisor para detectar pisadas
        if (myCollider != null) myCollider.enabled = true;
        
        // Muda animação (de enterrada para antena para fora)
        if (animator != null) animator.SetTrigger("Armed");
        
        Debug.Log("PotatoMine: Armada e pronta!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Segurança: Só explode se estiver armada
        if (!isArmed) return;

        // DETECÇÃO UNIVERSAL:
        // Verifica se o objeto que pisou é um zumbi (ou parte de um)
        zombie scriptZumbi = collision.GetComponentInParent<zombie>();

        if (scriptZumbi != null)
        {
            Explode();
        }
    }

    void Explode()
    {
        // 1. Cria o objeto da explosão no mesmo lugar
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // 2. DESTRÓI A MINA BATATA IMEDIATAMENTE
        // Isso faz ela sumir do cenário
        Destroy(gameObject);
    }
}