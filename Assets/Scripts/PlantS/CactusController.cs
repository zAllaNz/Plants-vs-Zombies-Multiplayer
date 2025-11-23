using System.Collections;
using UnityEngine;

public class CactusController : MonoBehaviour
{
    [Header("Game Elements")]
    public GameObject spikePrefab;
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

        // Só atira se tiver um alvo e se o tempo de recarga já passou
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
        }
        else
        {
            hasTarget = false;
        }
    }

    void Shoot()
    {
        // animator.SetTrigger("Shoot");

        Instantiate(spikePrefab, shootingPoint.position, shootingPoint.rotation);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(shootingPoint.position, (Vector2)shootingPoint.position + Vector2.right * detectionRange);
    }
}
