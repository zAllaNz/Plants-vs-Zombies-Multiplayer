using UnityEngine;

public class PeaController : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 20;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // MUDANÇA: Não verificamos mais a Tag primeiro.
        // Tentamos pegar o script 'zombie' diretamente no objeto ou nos pais dele.
        zombie scriptZumbi = collision.GetComponentInParent<zombie>();

        // Se encontrou o script, significa que É um zumbi válido
        if (scriptZumbi != null)
        {
            scriptZumbi.tomarDano(damage);
            Destroy(gameObject); // Destrói a ervilha
        }
        // Se bateu em algo que NÃO tem o script zombie (ex: outra planta), apenas ignora e passa reto.
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}