
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
    // partes em que a planta morre ou desaparece 
    private void Die() // morre
    {
        Debug.Log("A planta morreu!");
        // O Adicionar uma animação de morte ou efeito de partículas aqui futuramente
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.RemoverPlantaDaLista(this.gameObject);
        }
    }

    private void Start()
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.AdicionarPlantaNaLista(this.gameObject);

        }
    }

    private void OnMouseDown()
    {
        // Verifica se o GameManager existe e se ele está no modo de mira
        if (GameManager.instance != null && GameManager.instance.modoMiraBungeeAtivo)
        {
            // Se sim, avisa ao GameManager que ESTA planta foi o alvo escolhido
            GameManager.instance.ConfirmarAlvoBungee(this.gameObject);
        }
    }
}