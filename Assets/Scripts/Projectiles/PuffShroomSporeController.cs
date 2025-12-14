using UnityEngine;

public class PuffShroomSporeController : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 20;
    
    [Header("Alcance do Tiro")]
    public float maxDistance = 4.0f; // Deve ser um pouco maior que o detectionRange da planta

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Move para a direita
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Se viajar mais que o permitido, desaparece (mecânica única do Puff-shroom)
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Lógica Universal: Pega script no pai ou no filho
        zombie scriptZumbi = collision.GetComponentInParent<zombie>();

        if (scriptZumbi != null)
        {
            scriptZumbi.tomarDano(damage);
            Destroy(gameObject); 
        }
    }
}