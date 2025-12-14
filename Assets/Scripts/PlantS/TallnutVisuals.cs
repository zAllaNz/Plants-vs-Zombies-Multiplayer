using UnityEngine;

public class TallnutVisuals : MonoBehaviour
{
    private PlantHealth healthScript; 
    private Animator animator;
    private float maxHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Tenta pegar o script de vida no próprio objeto OU no objeto pai (por segurança)
        healthScript = GetComponentInParent<PlantHealth>();

        if (healthScript == null)
        {
            // Se der este erro, você esqueceu de colocar o script PlantHealth no Tallnut!
            Debug.LogError("ERRO CRÍTICO: TallnutVisuals não encontrou o PlantHealth!");
            return;
        }

        // Salva a vida inicial
        maxHealth = healthScript.saude; 
    }

    void Update()
    {
        // Se algo estiver faltando, não roda o resto para não dar erro NullReference
        if (healthScript == null || animator == null) return;

        UpdateAnimationState();
    }

    void UpdateAnimationState()
    {
        int vidaAtual = healthScript.saude;
        float porcentagem = (float)vidaAtual / maxHealth;

        int novoEstado = 0;

        if (porcentagem > 0.5f)       novoEstado = 0; // Cheio
        else if (porcentagem > 0.25f) novoEstado = 1; // Rachado
        else                          novoEstado = 2; // Quebrado

        animator.SetInteger("HealthState", novoEstado);
    }
}