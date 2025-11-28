using UnityEngine;

public class CactusController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject spikePrefab;
    public Transform shootingPoint;
    public LayerMask zombieLayer;

    [Header("Atributos")]
    public float fireRate = 1.0f;
    public float detectionRange = 10f;

    private float nextFireTime = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (ZombieInLane() && Time.time >= nextFireTime)
        {
            if (animator != null) animator.SetTrigger("Attack");
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    bool ZombieInLane()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.right, detectionRange, zombieLayer);
        return hit.collider != null;
    }

    void Shoot()
    {
        Instantiate(spikePrefab, shootingPoint.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        if (shootingPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(shootingPoint.position, shootingPoint.position + Vector3.right * detectionRange);
        }
    }
}
