using UnityEngine;

public class PeashooterController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject peaPrefab;
    public Transform shootingPoint;
    public LayerMask zombieLayer; // Confirme se está selecionado 'zombies' no Inspector

    [Header("Atributos")]
    public float fireRate = 1.5f;
    public float detectionRange = 10f;

    private float nextFireTime = 0f;
    private Animator animator;
    
    // Variável de controle de estado
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. O Raio detecta se tem alguém na linha
        bool zombieDetected = CheckForZombie();

        // 2. Se tem zumbi...
        if (zombieDetected)
        {
            // Entra no estado de ataque
            isAttacking = true;

            // Verifica o tempo de recarga (Cooldown)
            if (Time.time >= nextFireTime)
            {
                // MANDA O ANIMATOR RODAR A ANIMAÇÃO
                // A animação vai sair do Idle -> Attack imediatamente
                if (animator != null)
                {
                    animator.SetTrigger("Attack");
                }
                
                // Reseta o relógio para o próximo tiro
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            // 3. Se NÃO tem zumbi, desliga o estado de ataque
            isAttacking = false;
            // O Animator voltará para Idle automaticamente graças ao "Has Exit Time" que configuramos
        }
    }

    bool CheckForZombie()
    {
        // Lança o raio. Se bater em algo na layer 'zombies', retorna verdadeiro.
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.right, detectionRange, zombieLayer);
        return hit.collider != null;
    }

    // --- IMPORTANTE ---
    // Esta função DEVE ser chamada pelo "Animation Event" na janela de Animation
    // exatamente no frame que a planta cospe.
    public void DispararProjetil()
    {
        // Só cria a ervilha se ainda estivermos em modo de ataque.
        // Isso evita tiros fantasmas se o zumbi morrer no meio da animação.
        if (isAttacking)
        {
            Instantiate(peaPrefab, shootingPoint.position, Quaternion.identity);
        }
    }

    // Desenha a linha verde para você ver o alcance
    private void OnDrawGizmos()
    {
        if (shootingPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(shootingPoint.position, shootingPoint.position + Vector3.right * detectionRange);
        }
    }
}