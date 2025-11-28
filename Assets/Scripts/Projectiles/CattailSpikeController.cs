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
        // Verifica se tem o script zombie
        zombie scriptZumbi = collision.GetComponentInParent<zombie>();

        if (scriptZumbi != null)
        {
            scriptZumbi.tomarDano(damage);
            Destroy(gameObject);
        }
    }
}