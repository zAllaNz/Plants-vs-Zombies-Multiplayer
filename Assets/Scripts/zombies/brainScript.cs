using UnityEngine;
using UnityEngine.Rendering;

public class brainScript : MonoBehaviour
{
    public int BrainValor = 50;
    public float lifetimeBrain = 12.0f;

    public float minY = -3f; // A altura mínima que ele pode parar
    public float maxY = 4f;  // A altura máxima que ele pode parar
    public float fallingGravityScale = 0.1f;

    // variaveis internas 

    private float stopFallingHeight;
    private Rigidbody2D rb;
    private bool deveriaCair = false;
    private bool hasLanded = false;
    private GameManager gameManager;

    private void Start()
    {
        Destroy(gameObject, lifetimeBrain);
        stopFallingHeight = Random.Range(minY, maxY);
    }

    void Awake()
    {
       
        // Conecta o cérebro ao GameManager e ao Rigidbody2D.
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>(); 

        // Se o Rigidbody2D existir, já ativamos a gravidade, pois ele sempre vai cair.
        if (rb != null)
        {
            rb.gravityScale = fallingGravityScale;
        }
    }


    void Update()
    {
        // Se o cérebro deve cair, ainda não pousou, e atingiu a altura de parada...
        if (deveriaCair && !hasLanded && transform.position.y <= stopFallingHeight)
        {
            // ...ele para de se mover.
            hasLanded = true;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
        }
    }

    private void Collect()
    {
        if (gameManager != null)
        {
            gameManager.Addbrains(BrainValor);
        }
        Destroy(gameObject);
    }

    private void OnMouseEnter()
    {
        Collect();
    }

}





