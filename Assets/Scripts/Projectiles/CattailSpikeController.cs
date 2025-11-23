using UnityEngine;

public class CattailSpikeController : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    // O alvo que o projétil deve seguir. Será definido pelo Cattail ao atirar.
    private Transform target;

    // Método público para que o Cattail possa configurar o alvo
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        // Se não houver alvo (por exemplo, o zumbi já morreu), destrua o projétil.
        if (target == null)
        {
            Destroy(gameObject);
            return; // Encerra a execução do Update para evitar erros
        }

        // Calcula a direção do projétil até o alvo
        Vector3 direction = target.position - transform.position;

        // Move o projétil na direção calculada
        transform.position += direction.normalized * speed * Time.deltaTime;

        // Opcional: Faz o sprite do projétil apontar para o alvo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == target && collision.CompareTag("Zombie"))
        {
            //ZombieHealth zombieHealth = collision.GetComponent<ZombieHealth>();
            //if (zombieHealth != null)
            {
            //    zombieHealth.TakeDamage(damage);
            }
            
            Destroy(gameObject);
        }
    }
}