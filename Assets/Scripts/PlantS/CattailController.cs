using UnityEngine;

public class CattailController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject spikePrefab;
    public Transform shootingPoint;

    [Header("Atributos")]
    public float fireRate = 1.0f;

    private float nextFireTime = 0f;
    private Transform currentTarget;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Encontra o alvo a cada frame (ou poderia otimizar para rodar menos vezes)
        FindPriorityTarget();

        if (currentTarget != null && Time.time >= nextFireTime)
        {
            if (animator != null) animator.SetTrigger("Attack");
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FindPriorityTarget()
    {
        // Encontra TODOS os objetos com a tag Zombie
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        
        float minX = Mathf.Infinity;
        Transform bestTarget = null;

        foreach (GameObject z in zombies)
        {
            // Verifica se tem o script de zumbi (para garantir que é um inimigo válido)
            if (z.GetComponent<zombie>() != null)
            {
                // Procura o menor X (mais à esquerda = mais perigoso)
                if (z.transform.position.x < minX)
                {
                    minX = z.transform.position.x;
                    bestTarget = z.transform;
                }
            }
        }
        currentTarget = bestTarget;
    }

    void Shoot()
    {
        GameObject spike = Instantiate(spikePrefab, shootingPoint.position, Quaternion.identity);
        
        // Configura o alvo no projétil
        CattailSpikeController spikeScript = spike.GetComponent<CattailSpikeController>();
        if (spikeScript != null)
        {
            spikeScript.SetTarget(currentTarget);
        }
    }
}