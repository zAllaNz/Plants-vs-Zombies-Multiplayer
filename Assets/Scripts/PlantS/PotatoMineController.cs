using System.Collections;
using UnityEngine;

public class PotatoMineController : MonoBehaviour
{
    private enum MineState { Arming, Armed, Exploded }
    private MineState currentState;

    [Header("Setup")]
    public GameObject explosionPrefab;
    public Sprite armingSprite;
    public Sprite armedSprite;

    [Header("Stats")]
    public float armingTime = 10f;

    private SpriteRenderer spriteRenderer;
    private Collider2D myCollider;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();

        currentState = MineState.Arming;
        spriteRenderer.sprite = armingSprite;

        myCollider.enabled = false;

        StartCoroutine(ArmMine());
    }

    IEnumerator ArmMine()
    {
        yield return new WaitForSeconds(armingTime);

        if (currentState == MineState.Arming)
        {
            currentState = MineState.Armed;
            spriteRenderer.sprite = armedSprite;
            myCollider.enabled = true;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (currentState == MineState.Armed && collision.CompareTag("Zombie"))
        {
            Explode();
        }
    }

    void Explode()
    {
        currentState = MineState.Exploded;

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}