using UnityEngine;

public class sunScript : MonoBehaviour
{
    public int sunValue = 25;
    public float lifeTime = 10.0f;
    public float stopFallingHeight = -2.5f;
    public float fallingGravityScale = 0.1f;

    private Rigidbody2D rb;
    private bool shouldFall = false;
    private bool hasLanded = false;
    private GameManager gameManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
        if (rb != null)
        {
            rb.gravityScale = 0;
        }
    }

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(bool doFall)
    {
        shouldFall = doFall;
        if (shouldFall && rb != null)
        {
            rb.gravityScale = fallingGravityScale;
        }
    }

    void Update()
    {
        if (shouldFall && !hasLanded && transform.position.y <= stopFallingHeight)
        {
            hasLanded = true;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
        }
    }

    private void OnMouseEnter()
    {
        Collect();
    }

    private void Collect()
    {
        gameManager.AddSun(sunValue);
        Destroy(gameObject);
    }
}