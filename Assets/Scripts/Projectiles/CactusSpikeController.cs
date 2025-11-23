using UnityEngine;

public class CactusSpikeController : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 1;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            //ZombieHealth zombieHealth = collision.GetComponent<ZombieHealth>();
            //if (zombieHealth != null)
            {
            //    zombieHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
