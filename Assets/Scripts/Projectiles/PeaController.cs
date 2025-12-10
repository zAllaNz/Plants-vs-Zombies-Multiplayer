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
        // Tenta pegar o script específico do Zumbi do Balão
        zombie_balao zumbiBalao = collision.GetComponentInParent<zombie_balao>();

       
        // Se for um zumbi de balão E ele estiver voando...
        if (zumbiBalao != null && zumbiBalao.EstaVoando())
        {
            // ...o espinho ignora completamente!
            return;
        }  
        // Verifica se tem o script zombie genérico
        zombie scriptZumbi = collision.GetComponentInParent<zombie>();

        if (scriptZumbi != null)
        {
            scriptZumbi.tomarDano(damage);
            Destroy(gameObject); // O espinho só se destrói se acertar um zumbi no chão
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}