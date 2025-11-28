using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public int damage = 1800; // Dano massivo
    public float areaSize = 1.5f; 

    void Start()
    {
        // Causa dano imediatamente ao nascer
        Boom();
        
        // Some depois de 1 segundo
        Destroy(gameObject, 1.0f); 
    }

    void Boom()
    {
        Collider2D[] zombies = Physics2D.OverlapCircleAll(transform.position, areaSize);
        foreach (var z in zombies)
        {
            zombie script = z.GetComponentInParent<zombie>();
            if (script != null)
            {
                script.tomarDano(damage);
            }
        }
    }
}
