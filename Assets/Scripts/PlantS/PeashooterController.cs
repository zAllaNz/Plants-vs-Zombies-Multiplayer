using UnityEngine;

public class PeashooterController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject peaPrefab;
    public Transform shootingPoint;
    public LayerMask zombieLayer; // Configure isso no Inspector para "Zombies"

    [Header("Atributos")]
    public float fireRate = 1.5f;
    public float detectionRange = 10f; // Alcance da linha toda

    private float nextFireTime = 0f;

    void Update()
    {
        if (ZombieInLane() && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    bool ZombieInLane()
    {
        // Lança um raio invisível para detectar o zumbi na layer correta
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.right, detectionRange, zombieLayer);
        return hit.collider != null;
    }

    void Shoot()
    {
        // Cria a ervilha
        Instantiate(peaPrefab, shootingPoint.position, shootingPoint.rotation);
    }

    // Desenha a linha verde no editor para facilitar o debug
    private void OnDrawGizmos()
    {
        if (shootingPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(shootingPoint.position, shootingPoint.position + Vector3.right * detectionRange);
        }
    }
}