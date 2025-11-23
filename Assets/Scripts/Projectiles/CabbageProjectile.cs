using UnityEngine;

public class CabbageProjectile : MonoBehaviour
{
    public float travelDuration = 0.8f;
    public float arcHeight = 3f;        
    public int damage = 2;              

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float startTime;
    private GameObject targetZombie;

    public void Initialize(Vector3 startPos, GameObject target)
    {
        startPosition = startPos;
        targetZombie = target;

        transform.position = startPosition;
        startTime = Time.time;
    }

    void Update()
    {
        if (targetZombie == null)
        {
            Destroy(gameObject);
            return;
        }

        targetPosition = targetZombie.transform.position;

        float t = (Time.time - startTime) / travelDuration;

        Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, t);

        float arc = arcHeight * (Mathf.Sin(t * Mathf.PI));

        currentPos.y += arc;

        transform.position = currentPos;

        if (t >= 1f)
        {
            if (targetZombie != null)
            {
                //ZombieHealth zombieHealth = targetZombie.GetComponent<ZombieHealth>();
                //if (zombieHealth != null)
                {
                //    zombieHealth.TakeDamage(damage);
                }
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie") && collision.gameObject == targetZombie)
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