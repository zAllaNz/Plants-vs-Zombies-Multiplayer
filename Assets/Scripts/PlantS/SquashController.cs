using System.Collections;
using System.Linq;
using UnityEngine;

public class SquashController : MonoBehaviour
{
    [Header("Configuração")]
    public float detectionRadius = 1.5f; // Raio curto (1 bloco)
    public int damage = 500; // Instakill
    public LayerMask zombieLayer;

    [Header("Pulo")]
    public float jumpDuration = 0.5f; // Tempo que leva para cair no zumbi
    public float jumpHeight = 2.0f;   // Altura visual do pulo

    private bool isActing = false; // Para não ativar duas vezes
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isActing)
        {
            DetectZombie();
        }
    }

    void DetectZombie()
    {
        Collider2D[] zombies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, zombieLayer);

        // Pega o zumbi mais próximo
        Transform target = null;
        float minDist = Mathf.Infinity;

        foreach (var z in zombies)
        {
            // Validação Universal de Zumbi
            if (z.GetComponentInParent<zombie>() != null)
            {
                float dist = Vector2.Distance(transform.position, z.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    target = z.transform;
                }
            }
        }

        if (target != null)
        {
            StartCoroutine(JumpAndSmash(target));
        }
    }

    IEnumerator JumpAndSmash(Transform targetZombie)
    {
        isActing = true;
        
        // 1. Inicia Animação de Pulo
        if (animator) animator.SetTrigger("Jump");

        Vector3 startPos = transform.position;
        // Pega a posição X do zumbi, mas mantém o Y do chão (assumindo que o zumbi está no chão)
        Vector3 endPos = new Vector3(targetZombie.position.x, startPos.y, startPos.z);

        float elapsed = 0;

        // 2. Move o Squash em arco (Parábola) até o alvo
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpDuration;

            // Interpolação Linear para mover para o lado
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
            
            // Adiciona altura (Seno) para fazer o arco
            currentPos.y += Mathf.Sin(t * Mathf.PI) * jumpHeight;

            transform.position = currentPos;
            yield return null;
        }

        // Garante que chegou no chão
        transform.position = endPos;

        // 3. Causa Dano e Toca Animação de Impacto
        Smash();
    }

    void Smash()
    {
        if (animator) animator.SetTrigger("Smash");

        // Área de dano no ponto de impacto
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, 1.0f, zombieLayer);
        
        foreach (var enemy in enemies)
        {
            zombie scriptZumbi = enemy.GetComponentInParent<zombie>();
            if (scriptZumbi != null)
            {
                scriptZumbi.tomarDano(damage);
            }
        }

        // Destroi após o fim da animação de smash (aprox 0.5s)
        Destroy(gameObject, 0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}