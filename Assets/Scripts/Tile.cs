// Tile.cs (versão para colocar ZUMBIS)
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isOccupied = false;
    private void OnMouseDown()
    {
        GameObject plantPrefab = PlantingManager.Instance.GetSelectedPlantPrefab();

        if (plantPrefab != null && !isOccupied)
        {
            Instantiate(plantPrefab, transform.position, Quaternion.identity);
            isOccupied = true;
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

    /*private void OnMouseDown()
    {
        // Pega o prefab do ZombieManager
        GameObject zombiePrefab = ZombieManager.Instance.GetSelectedZombiePrefab();

        if (zombiePrefab != null && !isOccupied)
        {
            Instantiate(zombiePrefab, transform.position, Quaternion.identity);
            isOccupied = true;

            // Avisa o ZombieManager que o zumbi foi colocado
            ZombieManager.Instance.ZombieWasPlaced();
        }
        else if (isOccupied)
        {
            Debug.Log("Este slot já está ocupado!");
        }
        else
        {
            Debug.Log("Nenhum zumbi selecionado para colocar.");
        }
    }
    */
}