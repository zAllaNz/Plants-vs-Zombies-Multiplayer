using UnityEngine;

public class PuffShroomController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject sporePrefab;
    public Transform shootingPoint;
    public LayerMask zombieLayer;

    [Header("Atributos")]
    public float fireRate = 1.5f;
    public float detectionRange = 3.5f; // Alcance curto (aprox 1/3 da tela)

    private float nextFireTime = 0f;

    void Update()
    {
        if (ZombieInRange() && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    bool ZombieInRange()
    {
        // Raio curto
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.right, detectionRange, zombieLayer);
        return hit.collider != null;
    }

    void Shoot()
    {
        Instantiate(sporePrefab, shootingPoint.position, shootingPoint.rotation);
    }

    private void OnDrawGizmos()
    {
        if (shootingPoint != null)
        {
            Gizmos.color = Color.magenta; // Magenta para alcance curto
            Gizmos.DrawLine(shootingPoint.position, shootingPoint.position + Vector3.right * detectionRange);
        }
    }
}
