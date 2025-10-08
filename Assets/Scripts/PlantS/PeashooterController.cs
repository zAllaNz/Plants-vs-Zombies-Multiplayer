using UnityEngine;

public class PeashooterController : MonoBehaviour
{
    [Header("Game Elements")]
    public GameObject peaPrefab;
    public Transform shootingPoint;
    public LayerMask zombieLayer;

    [Header("Shooting Stats")]
    public float fireRate = 1.5f;
    public float detectionRange = 10f;

    private bool hasTarget = false;
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

        hasTarget = (hit.collider != null);
    }

    void Shoot()
    {

        Instantiate(peaPrefab, shootingPoint.position, shootingPoint.rotation);
    }

    private void OnDrawGizmos()
    {
        if (shootingPoint == null) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(shootingPoint.position, (Vector2)shootingPoint.position + Vector2.right * detectionRange);
    }
}