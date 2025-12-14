using UnityEngine;

public class CactusSpikeController : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 20;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Lógica Universal: Pega o zumbi (pai ou filho)
        zombie scriptZumbi = collision.GetComponentInParent<zombie>();

        if (scriptZumbi != null)
        {
            // O script do zumbi_balao já sabe que o primeiro dano estoura o balão
            scriptZumbi.tomarDano(damage);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}