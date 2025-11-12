
using UnityEngine;

public class ProjetilCacto : MonoBehaviour
{
    public float velocidade = 10f;
    public int dano = 25;

    void Update()
    {
        // Move o projétil para a direita
        transform.Translate(Vector2.right * velocidade * Time.deltaTime);

        // Auto-destrói se sair da tela
        if (transform.position.x > 20f)
        {
            Destroy(gameObject);
        }
    }

    // Chamado quando colide com outro Collider 2D
    void OnTriggerEnter2D(Collider2D other)
    {
        // Tenta pegar o script "zombie" do objeto em que colidiu
        zombie zumbi = other.GetComponent<zombie>();

        // Se for um zumbi...
        if (zumbi != null)
        {
            // Causa dano nele
            zumbi.tomarDano(dano);
            // E se destrói
            Destroy(gameObject);
        }
    }
}