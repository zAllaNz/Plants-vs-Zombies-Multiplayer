using System.Collections;
using System.Linq;
using UnityEngine;

public class SquashController : MonoBehaviour
{
    private enum SquashState { Idle, Preparing, Jumping, Smashing }
    private SquashState currentState;

    [Header("Stats")]
    public float detectionRadius = 2.0f;
    public int damage = 100; 
    public float jumpHeight = 4f; 
    public float jumpDuration = 0.5f; 
    public LayerMask zombieLayer;

    private Transform targetZombie;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool hasTarget = false;

    void Start()
    {
        currentState = SquashState.Idle;
        startPosition = transform.position;
    }

    void Update()
    {
        if (currentState == SquashState.Idle)
        {
            DetectZombie();
        }
    }

    void DetectZombie()
    {
        Collider2D[] zombiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, zombieLayer);

        if (zombiesInRange.Length > 0)
        {
            targetZombie = zombiesInRange.OrderBy(z => Vector2.Distance(transform.position, z.transform.position)).FirstOrDefault().transform;

            if (targetZombie != null)
            {
                hasTarget = true;
                currentState = SquashState.Preparing;

                StartCoroutine(JumpAndSmash());
            }
        }
    }

    IEnumerator JumpAndSmash()
    {

        currentState = SquashState.Jumping;
        targetPosition = new Vector3(targetZombie.position.x, startPosition.y, startPosition.z); // Pega a posição X do zumbi, mas mantém a altura Y do chão

        float elapsedTime = 0;

        while (elapsedTime < jumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / jumpDuration;

            Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, progress);

            currentPos.y += jumpHeight * Mathf.Sin(progress * Mathf.PI);

            transform.position = currentPos;
            yield return null;
        }

        transform.position = targetPosition;

        Smash();
    }

    void Smash()
    {
        currentState = SquashState.Smashing;

        Collider2D[] zombiesToSmash = Physics2D.OverlapBoxAll(transform.position, new Vector2(1, 2), 0, zombieLayer);

        foreach (var zombieCollider in zombiesToSmash)
        {
            //ZombieHealth health = zombieCollider.GetComponent<ZombieHealth>();
            //if (health != null)
            {
            //    health.TakeDamage(damage);
            }
        }

        Destroy(gameObject, 0.2f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if(currentState == SquashState.Smashing)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector2(1, 2));
        }
    }
}