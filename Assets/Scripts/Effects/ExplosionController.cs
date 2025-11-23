using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public int damage = 50;
    public float explosionRadius = 1.5f;
    public float lifespan = 0.5f;

    void Start()
    {
        DealAreaDamage();

        Destroy(gameObject, lifespan);
    }

    void DealAreaDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in colliders)
        {
            // Se o objeto atingido tiver a tag "Zombie"...
            if (hit.CompareTag("Zombie"))
            {
                //ZombieHealth zombieHealth = hit.GetComponent<ZombieHealth>();
                //if (zombieHealth != null)
                {
                    // Causa dano massivo
                //    zombieHealth.TakeDamage(damage);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
