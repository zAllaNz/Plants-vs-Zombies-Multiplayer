using UnityEngine;
using System.Collections;

public class RepeaterController : MonoBehaviour
{
    [Header("Configuração")]
    public GameObject peaPrefab;
    public Transform shootingPoint;
    public LayerMask zombieLayer;

    [Header("Atributos")]
    public float fireRate = 2.0f;           
    public float detectionRange = 10f;
    public float delayBetweenShots = 0.25f; 

    private float nextFireTime = 0f;
    private Animator animator;
    private bool isShootingRoutineActive = false; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Só processa lógica se NÃO estiver atirando no momento
        if (!isShootingRoutineActive)
        {
            if (CheckForZombie())
            {
                // Verifica o tempo de recarga
                if (Time.time >= nextFireTime)
                {
                    Atacar();
                }
            }
            else
            {
                // Se não tem zumbi, garante que o trigger de ataque esteja desligado
                // para não disparar um ataque atrasado se um zumbi aparecer de repente
                if(animator != null) animator.ResetTrigger("Attack");
            }
        }
    }

    void Atacar()
    {
        // Define o tempo do PRÓXIMO ataque
        nextFireTime = Time.time + fireRate;

        if (animator != null)
        {
            // Resetamos o trigger antes de setar de novo para evitar acúmulo
            animator.ResetTrigger("Attack"); 
            animator.SetTrigger("Attack");
        }
    }

    bool CheckForZombie()
    {
        RaycastHit2D hit = Physics2D.Raycast(shootingPoint.position, Vector2.right, detectionRange, zombieLayer);
        return hit.collider != null;
    }

    // Chamado pelo Evento de Animação
    public void DispararProjetil()
    {
        if (!isShootingRoutineActive)
        {
            StartCoroutine(RajadaDupla());
        }
    }

    IEnumerator RajadaDupla()
    {
        isShootingRoutineActive = true;

        // Tiro 1
        CriarErvilha();

        // Espera o intervalo entre ervilhas
        yield return new WaitForSeconds(delayBetweenShots);

        // Tiro 2
        CriarErvilha();

        // Espera um pouquinho para garantir que a animação visual terminou antes de liberar a lógica
        // Isso ajuda a sincronizar se a animação for muito curta
        yield return new WaitForSeconds(0.1f);

        isShootingRoutineActive = false;
    }

    void CriarErvilha()
    {
        Instantiate(peaPrefab, shootingPoint.position, Quaternion.identity);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if(shootingPoint)
            Gizmos.DrawLine(shootingPoint.position, shootingPoint.position + Vector3.right * detectionRange);
    }
}