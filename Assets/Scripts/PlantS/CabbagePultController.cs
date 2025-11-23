using UnityEngine;

public class CabbagePultController : MonoBehaviour
{
    [Header("Game Elements")]
    public GameObject cabbageProjectilePrefab;
    public Transform shootingPoint;
    public LayerMask zombieLayer;

    [Header("Shooting Stats")]
    public float fireRate = 2.0f;
    public float detectionRange = 10f;

    private bool hasTarget = false;
    private GameObject currentTargetZombie;
    private float nextFireTime = 0f;

    void Update()
    {
        CheckForZombieInLane();

        if (hasTarget && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void CheckForZombieInLane()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.right, detectionRange, zombieLayer);

        if (hit.collider != null)
        {
            hasTarget = true;
            currentTargetZombie = hit.collider.gameObject;
        }
        else
        {
            hasTarget = false;
            currentTargetZombie = null;
        }
    }

    void Shoot()
    {
        if (currentTargetZombie == null) return;

        // Inserir animação de arremesso aqui

        GameObject cabbageGO = Instantiate(cabbageProjectilePrefab, shootingPoint.position, Quaternion.identity);

        cabbageGO.GetComponent<CabbageProjectile>().Initialize(shootingPoint.position, currentTargetZombie);
    }

    private void OnDrawGizmos()
    {
        if (shootingPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(shootingPoint.position, (Vector2)shootingPoint.position + Vector2.right * detectionRange);
    }
}