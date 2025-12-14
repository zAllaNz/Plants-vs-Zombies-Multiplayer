using UnityEngine;
using System.Collections; // Necessário para Corrotinas

public class PuffShroomController : MonoBehaviour
{
    [Header("Configuração Essencial")]
    public GameObject sporePrefab;   // Prefab do Esporo
    public Transform shootingPoint;  // Ponto de saída (boca)
    public LayerMask zombieLayer;    // Layer "zombies"

    [Header("Atributos")]
    public float fireRate = 1.5f;       // Cadência de tiro
    public float detectionRange = 3.5f; // Alcance curto (Peashooter é 10, esse é ~3.5)

    [Header("Sincronia Visual")]
    public float attackDelay = 0.2f;    // Tempo para o esporo sair após iniciar a animação

    private float nextFireTime = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (sporePrefab == null)
            Debug.LogError("ERRO: Puff-shroom sem Prefab do Esporo no Inspector!");
    }

    void Update()
    {
        // Debug Visual: Linha Magenta para mostrar o alcance curto
        Debug.DrawRay(shootingPoint.position, Vector2.right * detectionRange, Color.magenta);

        // 1. Verifica Cooldown
        if (Time.time >= nextFireTime)
        {
            // 2. Procura zumbi (Lógica do Peashooter: Raycast Simples)
            if (CheckForZombieInRange())
            {
                // 3. Inicia rotina de tiro (Lógica do Cabbage: Corrotina)
                StartCoroutine(ShootRoutine());
                
                // Define o tempo do PRÓXIMO tiro
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                // Se não tem zumbi, garante que a animação pare
                if(animator != null) animator.ResetTrigger("Attack");
            }
        }
    }

    bool CheckForZombieInRange()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.right, detectionRange, zombieLayer);
        return hit.collider != null;
    }

    IEnumerator ShootRoutine()
    {
        // 1. Toca a animação IMEDIATAMENTE
        if (animator != null)
        {
            animator.ResetTrigger("Attack"); // Limpa comandos antigos
            animator.SetTrigger("Attack");
        }

        // 2. ESPERA o tempo visual do "sopro"
        yield return new WaitForSeconds(attackDelay);

        // 3. Cria o esporo
        if (sporePrefab != null)
        {
            Instantiate(sporePrefab, shootingPoint.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        if (shootingPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(shootingPoint.position, shootingPoint.position + Vector3.right * detectionRange);
        }
    }
}