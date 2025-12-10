using UnityEngine;

public class PuffShroomSporeController : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 20;
    public float maxDistance = 4.0f; 

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Tenta pegar o script específico do Zumbi do Balão
        zombie_balao zumbiBalao = collision.GetComponentInParent<zombie_balao>();

       
        // Se for um zumbi de balão E ele estiver voando...
        if (zumbiBalao != null && zumbiBalao.EstaVoando())
        {
      
            return;
        }

        // Verifica se tem o script zombie genérico
        zombie scriptZumbi = collision.GetComponentInParent<zombie>();

        if (scriptZumbi != null)
        {
            scriptZumbi.tomarDano(damage);
            Destroy(gameObject); 
        }
    }
}