using UnityEngine;

public class CactusSpikeController : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;

    void Update()
    {
        // Move sempre para a direita
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se bateu em algo com a tag "Zombie"
        if (collision.CompareTag("Zombie"))
        {
            // Tenta pegar o script "zombie" do grupo
            zombie scriptZumbi = collision.GetComponent<zombie>();

            if (scriptZumbi != null)
            {
                scriptZumbi.tomarDano(damage);
            }
            
            // Destrói o espinho após bater
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject); // Limpa memória se sair da tela
    }
}
