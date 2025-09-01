
using UnityEngine;

public class Plant : MonoBehaviour
{
    public int health = 100; // A vida total da planta

    // Método público que o zumbi vai chamar para causar dano
    public void TakeDamage(int dano)
    {
        // Subtrai o dano da vida
        health -= dano;
        Debug.Log("Planta tomou " + dano + " de dano! Vida restante: " + health);

        // Verifica se a vida chegou a zero
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("A planta morreu!");
        // O Adicionar uma animação de morte ou efeito de partículas aqui futuramente
        Destroy(gameObject);
    }
}