using System.Collections;
using UnityEngine;

public class RepeaterController : MonoBehaviour
{
    [Header("Game Elements")]
    public GameObject peaPrefab;
    public Transform shootingPoint;
    public LayerMask zombieLayer;

    [Header("Shooting Stats")]
    public float fireRate = 1.5f;
    public float delayBetweenPeas = 0.2f;
    public float detectionRange = 10f;

    private bool hasTarget = false;
    private bool isShooting = false;

    void Update()
    {
        CheckForZombieInLane();

        if (hasTarget && !isShooting)
        {
            StartCoroutine(ShootTwoPeas());
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

    IEnumerator ShootTwoPeas()
    {
        isShooting = true;

        // Primeiro tiro
        Instantiate(peaPrefab, shootingPoint.position, shootingPoint.rotation);

        // Pequeno atraso
        yield return new WaitForSeconds(delayBetweenPeas);

        // Segundo tiro
        Instantiate(peaPrefab, shootingPoint.position, shootingPoint.rotation);

        yield return new WaitForSeconds(fireRate - delayBetweenPeas);

        isShooting = false;
    }

    private void OnDrawGizmos()
    {
        if (shootingPoint == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(shootingPoint.position, (Vector2)shootingPoint.position + Vector2.right * detectionRange);
    }
}