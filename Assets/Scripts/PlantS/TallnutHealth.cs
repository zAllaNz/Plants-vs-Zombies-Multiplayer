using UnityEngine;

public class TallnutHealth : MonoBehaviour
{
    public int maxHealth = 4000; // Muita vida
    private int currentHealth;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    // Este método precisa ter O MESMO NOME que os zumbis chamam
    // No script 'zombie.cs', eles buscam 'Plant' e chamam 'TakeDamage'.
    // Certifique-se de que este script substitui o 'Plant.cs' ou ajuste o zumbi para achar este.
    // O ideal é que Tallnut também tenha o componente 'Plant' ou herde dele.
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        UpdateVisuals();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateVisuals()
    {
        if (animator == null) return;

        float healthPercent = (float)currentHealth / maxHealth;

        // Configurar Parameters no Animator: "DamageState" (Int)
        // 0 = Intacta, 1 = Rachada, 2 = Quase morta
        if (healthPercent > 0.66f)
        {
            animator.SetInteger("DamageState", 0);
        }
        else if (healthPercent > 0.33f)
        {
            animator.SetInteger("DamageState", 1);
        }
        else
        {
            animator.SetInteger("DamageState", 2);
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}