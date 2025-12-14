using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public int damage = 1800; // Dano Massivo (Mata qualquer coisa)
    public float areaSize = 1.5f; // Raio da explosão (quase 1 quadrado)

    void Start()
    {
        // Causa dano assim que nasce
        Boom();
        
        // Some visualmente após a animação (ex: 1 segundo)
        Destroy(gameObject, 1.0f); 
    }

    void Boom()
    {
        // Pega tudo numa área circular
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, areaSize);

        foreach (var hit in hits)
        {
            // DETECÇÃO UNIVERSAL PARA DANO EM ÁREA
            zombie scriptZumbi = hit.GetComponentInParent<zombie>();
            
            if (scriptZumbi != null)
            {
                scriptZumbi.tomarDano(damage);
            }
        }
    }

    // Desenha a área vermelha no editor para você ver o tamanho
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, areaSize);
    }
}
