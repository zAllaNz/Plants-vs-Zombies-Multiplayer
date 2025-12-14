using UnityEngine;

public class PlantHealth : MonoBehaviour
{
    [Header("Atributos de Vida")]
    public int saude = 100; 

    // O zumbi chama esta função quando morde
    public void TakeDamage(int dano)
    {
        saude -= dano;

        // VERIFICAÇÃO CRÍTICA: Se a vida zerou (ou ficou negativa)
        if (saude <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Debug para você ver no console que funcionou
        Debug.Log(gameObject.name + " morreu e foi excluido!");

        // COMANDO DE EXCLUSÃO
        // Destrói o objeto da planta imediatamente
        Destroy(gameObject);
    }
}