using UnityEngine;

public class PuffShroomSporeController : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    public float maxDistance = 3.5f;

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
        if (collision.CompareTag("Zombie"))
        {
            //ZombieHealth zombieHealth = collision.GetComponent<ZombieHealth>();
            //if (zombieHealth != null)
            {
            //    zombieHealth.TakeDamage(damage);
            }
            // Destrói ao colidir com um zumbi
            Destroy(gameObject);
        }
    }
}