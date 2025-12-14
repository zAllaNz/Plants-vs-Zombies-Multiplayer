using UnityEngine;

public class CattailSpikeController : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 20;

    private Transform target; // O alvo que ele deve perseguir

    // Chamado pela planta para definir quem perseguir
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        // Se o alvo morreu ou sumiu, destrói o espinho
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // 1. Calcula a direção até o alvo
        Vector3 direction = target.position - transform.position;

        // 2. Move o espinho na direção do alvo
        transform.position += direction.normalized * speed * Time.deltaTime;

        // 3. Faz o espinho "olhar" para o alvo (Rotação)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // -90 se o sprite estiver em pé
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // LÓGICA UNIVERSAL: Pega o script no objeto ou no pai
        zombie scriptZumbi = collision.GetComponentInParent<zombie>();

        // Verifica se é o alvo certo OU se bateu em outro zumbi no caminho
        if (scriptZumbi != null)
        {
            scriptZumbi.tomarDano(damage);
            Destroy(gameObject);
        }
    }
}