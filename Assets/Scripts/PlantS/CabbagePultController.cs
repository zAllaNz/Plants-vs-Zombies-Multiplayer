using UnityEngine;
using System.Collections; // Necessário para usar Corrotinas

public class CabbagePultController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject cabbageProjectilePrefab;
    public Transform shootingPoint;
    public LayerMask zombieLayer;

    [Header("Atributos")]
    public float fireRate = 2.0f;
    public float detectionRange = 10f;
    
    [Header("Sincronia Visual")]
    public float attackDelay = 0.5f; // TEMPO DE ESPERA PARA O TIRO SAIR

    private float nextFireTime = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.DrawRay(shootingPoint.position, Vector2.right * detectionRange, Color.red);

        if (Time.time >= nextFireTime)
        {
            GameObject alvo = CheckForZombieInLane();

            if (alvo != null)
            {
                // Inicia a rotina de tiro sincronizado
                StartCoroutine(ShootRoutine(alvo));
                
                // Define o tempo do próximo tiro
                nextFireTime = Time.time + fireRate;
            }
            else
            {
                if (animator != null) animator.SetBool("IsAttacking", false);
            }
        }
    }

    GameObject CheckForZombieInLane()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.right, detectionRange, zombieLayer);
        
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    IEnumerator ShootRoutine(GameObject targetZombie)
    {
        // 1. Toca a animação IMEDIATAMENTE
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // 2. ESPERA o tempo exato para o braço da catapulta subir
        yield return new WaitForSeconds(attackDelay);

        // 3. Verifica se o zumbi ainda está vivo antes de criar o projétil
        if (targetZombie != null)
        {
            // Cria o repolho
            GameObject projectile = Instantiate(cabbageProjectilePrefab, shootingPoint.position, Quaternion.identity);

            // Inicializa o projétil
            CabbageProjectile scriptProj = projectile.GetComponent<CabbageProjectile>();
            if (scriptProj != null)
            {
                scriptProj.Initialize(shootingPoint.position, targetZombie);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (shootingPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(shootingPoint.position, shootingPoint.position + Vector3.right * detectionRange);
        }
    }
}