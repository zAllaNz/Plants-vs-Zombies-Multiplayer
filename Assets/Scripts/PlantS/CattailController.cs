using UnityEngine;
using System.Collections; // Necessário para Corrotinas

public class CattailController : MonoBehaviour
{
    [Header("Configuração Essencial")]
    public GameObject spikePrefab;   // Prefab do Espinho
    public Transform shootingPoint;  // Ponto de saída
    public LayerMask zombieLayer;    // Layer "zombies"

    [Header("Atributos")]
    public float fireRate = 1.0f;    // Cadência de tiro
    public float globalRange = 50f;  // Alcance global
    
    [Header("Sincronia Visual")]
    public float attackDelay = 0.2f; // TEMPO DE ESPERA PARA O TIRO SAIR

    private float nextFireTime = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (spikePrefab == null)
            Debug.LogError("ERRO: Cattail sem Prefab do Espinho!");
    }

    void Update()
    {
        // 1. Encontra o zumbi prioritário
        Transform alvo = FindPriorityTarget();

        // 2. Se achou alguém e pode atirar
        if (alvo != null && Time.time >= nextFireTime)
        {
            // Inicia a rotina sincronizada
            StartCoroutine(ShootRoutine(alvo));
            
            // Define o próximo tempo de tiro
            nextFireTime = Time.time + fireRate;
        }
    }

    Transform FindPriorityTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, globalRange, zombieLayer);

        Transform melhorAlvo = null;
        float menorX = Mathf.Infinity;

        foreach (var hit in hits)
        {
            // Filtra zumbi válido (pai ou filho)
            if (hit.GetComponentInParent<zombie>() != null)
            {
                // Busca o menor X (mais à esquerda)
                if (hit.transform.position.x < menorX)
                {
                    menorX = hit.transform.position.x;
                    melhorAlvo = hit.transform;
                }
            }
        }
        return melhorAlvo;
    }

    IEnumerator ShootRoutine(Transform target)
    {
        // 1. Toca a animação IMEDIATAMENTE
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 2. ESPERA o tempo visual do disparo
        yield return new WaitForSeconds(attackDelay);

        // 3. Verifica se o alvo ainda existe (pode ter morrido na espera)
        if (target != null && spikePrefab != null)
        {
            // Cria o espinho
            GameObject spike = Instantiate(spikePrefab, shootingPoint.position, Quaternion.identity);

            // Configura o alvo do espinho
            CattailSpikeController spikeScript = spike.GetComponent<CattailSpikeController>();
            if (spikeScript != null)
            {
                spikeScript.SetTarget(target);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, globalRange);
    }
}