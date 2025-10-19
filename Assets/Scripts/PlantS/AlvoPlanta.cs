
using UnityEngine;

public class PlantaAlvo : MonoBehaviour // Ou 'public class Plant : MonoBehaviour'
{
    // ... (Seu código de vida da planta, etc., pode vir aqui) ...

    // Garante que a planta se registre no GameManager
    void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.AdicionarPlantaNaLista(this.gameObject);
        }
    }

    // Garante que a planta saia da lista ao ser destruída
    void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.RemoverPlantaDaLista(this.gameObject);
        }
    }


    // ESTA É A PARTE NOVA E IMPORTANTE:
    // Esta função é chamada automaticamente pelo Unity quando
    // este objeto (com um Collider) é clicado pelo mouse.
    private void OnMouseDown()
    {
        // 1. Verifica se o GameManager existe e se ele está no modo de mira
        if (GameManager.instance != null && GameManager.instance.modoMiraBungeeAtivo)
        {
            // 2. Se sim, avisa ao GameManager que ESTA planta foi o alvo escolhido
            GameManager.instance.ConfirmarAlvoBungee(this.gameObject);
        }
    }
}