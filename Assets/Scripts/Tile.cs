using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isOccupied = false; // Controla se o slot já tem uma planta

    // OnMouseDown é chamado quando o mouse é clicado sobre um Collider
    private void OnMouseDown()
    {
        // 1. Pergunta ao gerenciador qual prefab está selecionado
        GameObject plantPrefab = PlantingManager.Instance.GetSelectedPlantPrefab();

        // 2. Verifica se um prefab válido foi retornado e se o slot está livre
        if (plantPrefab != null && !isOccupied)
        {
            // 3. Instancia a planta na posição deste slot
            Instantiate(plantPrefab, transform.position, Quaternion.identity);

            // 4. Marca o slot como ocupado
            isOccupied = true;

            // 5. Informa ao gerenciador que o plantio foi concluído
            // Isso vai consumir os sóis e deselecionar a planta.
            PlantingManager.Instance.PlantWasPlaced();

            Debug.Log("Planta colocada em " + gameObject.name);
        }
        else if (isOccupied)
        {
            Debug.Log("Este slot já está ocupado!");
        }
        else
        {
            Debug.Log("Nenhuma planta selecionada para plantar.");
        }
    }
}