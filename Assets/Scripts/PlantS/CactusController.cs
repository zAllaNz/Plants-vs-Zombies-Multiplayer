using UnityEngine;
using System.Collections;

public class CactusController : MonoBehaviour
{
    [Header("Configuração Essencial")]
    public GameObject spikePrefab;   // Prefab do Espinho
    
    [Header("Pontos de Disparo")]
    public Transform shootPointNormal; // Arraste o ShootingPoint_Normal
    public Transform shootPointHigh;   // Arraste o ShootingPoint_High
    
    public LayerMask zombieLayer;      // Selecione "zombies"

    [Header("Atributos")]
    public float fireRate = 1.2f;      // Velocidade de ataque
    public float detectionRange = 10f; // Alcance do raio

    [Header("Sincronia Visual")]
    public float attackDelay = 0.2f;      // Tempo para o tiro NORMAL (Baixo)
    public float highAttackDelay = 0.5f;  // NOVO: Tempo para o tiro ALTO (Crescer + Atirar)

    private float nextFireTime = 0f;
    private Animator animator;
    private bool isStretched = false;  // Variável de controle de estado

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Verifica se tem BALÃO VOANDO na fileira
        bool balloonDetected = CheckForBalloon();

        // 2. Lógica de transição de estado
        if (balloonDetected)
        {
            // Se tem balão, estica!
            if (!isStretched)
            {
                isStretched = true;
                if(animator != null) animator.SetBool("IsStretched", true);
            }
        }
        else
        {
            // Se NÃO tem balão, encolhe!
            if (isStretched)
            {
                isStretched = false;
                if(animator != null) animator.SetBool("IsStretched", false);
            }
        }

        // 3. Verifica se deve atirar
        if (Time.time >= nextFireTime)
        {
            // Atira se tiver balão (prioridade) OU zumbi normal
            if (balloonDetected || CheckForNormalZombie())
            {
                StartCoroutine(ShootRoutine());
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                // Limpa o gatilho se não tiver ninguém
                if(animator != null) animator.ResetTrigger("Attack");
            }
        }
        
        // Debug
        if(shootPointNormal) Debug.DrawRay(shootPointNormal.position, Vector2.right * detectionRange, Color.green);
        if(shootPointHigh) Debug.DrawRay(shootPointHigh.position, Vector2.right * detectionRange, Color.cyan);
    }

    // Procura especificamente por Zumbi Balão Voando
    bool CheckForBalloon()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, detectionRange, zombieLayer);

        foreach (var hit in hits)
        {
            zombie_balao scriptBalao = hit.collider.GetComponentInParent<zombie_balao>();
            
            // Se achou o script E ele diz que está voando
            if (scriptBalao != null && scriptBalao.IsFlying()) 
            {
                return true; 
            }
        }
        return false;
    }

    bool CheckForNormalZombie()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootPointNormal.position, Vector2.right, detectionRange, zombieLayer);
        return hit.collider != null;
    }

    IEnumerator ShootRoutine()
    {
        // Toca a animação
        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Attack");
        }

        // --- LÓGICA DE TEMPO INTELIGENTE ---
        // Se estiver esticado (Modo Alto), usa o tempo maior (highAttackDelay)
        // Se estiver normal, usa o tempo curto (attackDelay)
        float tempoDeEspera = isStretched ? highAttackDelay : attackDelay;

        // Espera o tempo definido antes de criar o projétil
        yield return new WaitForSeconds(tempoDeEspera);

        if (spikePrefab != null)
        {
            // Define de onde sai o tiro
            Transform origem = isStretched ? shootPointHigh : shootPointNormal;
            
            if (origem != null)
            {
                Instantiate(spikePrefab, origem.position, Quaternion.identity);
            }
        }
    }
}
