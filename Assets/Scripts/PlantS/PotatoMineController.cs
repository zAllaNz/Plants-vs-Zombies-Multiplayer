using System.Collections;
using UnityEngine;

public class PotatoMineController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject explosionPrefab; // Precisa criar esse prefab
    public float armingTime = 14f; // Tempo para armar
    
    private bool isArmed = false;
    private Animator animator;
    private Collider2D myCollider; // Para desligar colisão enquanto arma

    void Start()
    {
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        
        // Começa "intangível" para os zumbis não ativarem antes da hora
        if (myCollider) myCollider.enabled = false;

        StartCoroutine(ArmRoutine());
    }

    IEnumerator ArmRoutine()
    {
        // Toca animação de "plantando/esperando" (padrão é Idle Unarmed)
        yield return new WaitForSeconds(armingTime);

        // ARMOU!
        isArmed = true;
        if (myCollider) myCollider.enabled = true; // Agora pode colidir
        
        // Muda animação para "Pronta"
        if (animator) animator.SetTrigger("Armed");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isArmed) return;

        // Verifica se é zumbi de forma universal
        if (collision.GetComponentInParent<zombie>() != null)
        {
            Explode();
        }
    }

    void Explode()
    {
        // Instancia a explosão (que vai dar o dano)
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        
        // Destroi a batata
        Destroy(gameObject);
    }
}