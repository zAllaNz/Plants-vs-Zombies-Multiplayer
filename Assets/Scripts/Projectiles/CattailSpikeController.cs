using UnityEngine;

public class CattailSpikeController : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 20;
    
    private Transform target; 

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        transform.position += direction.normalized * speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
}