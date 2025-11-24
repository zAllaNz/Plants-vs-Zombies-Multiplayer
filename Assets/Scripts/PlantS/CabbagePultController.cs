using UnityEngine;

public class CabbagePultController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject cabbageProjectilePrefab;
    public Transform shootingPoint;
    public LayerMask zombieLayer; // Selecione a layer onde estão os zumbis

    [Header("Atributos")]
    public float fireRate = 2.0f;
    public float detectionRange = 10f; 

    private float nextFireTime = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Detecta zumbi na linha
        GameObject alvo = CheckForZombieInLane();

        if (alvo != null && Time.time >= nextFireTime)
        {
            // Toca a animação
            if(animator != null) animator.SetTrigger("Attack");
            
            // Atira (passando o alvo)
            Shoot(alvo);
            
            nextFireTime = Time.time + fireRate;
        }
    }

    GameObject CheckForZombieInLane()
    {
        // Raio para direita
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.right, detectionRange, zombieLayer);
        
        if (hit.collider != null)
        {
            return hit.collider.gameObject; // Retorna o zumbi encontrado
        }
        return null;
    }

    void Shoot(GameObject targetZombie)
    {
        GameObject projectile = Instantiate(cabbageProjectilePrefab, shootingPoint.position, Quaternion.identity);
        
        // Inicializa o projétil passando o alvo para ele calcular o arco
        CabbageProjectile scriptProj = projectile.GetComponent<CabbageProjectile>();
        if(scriptProj != null)
        {
            scriptProj.Initialize(shootingPoint.position, targetZombie);
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