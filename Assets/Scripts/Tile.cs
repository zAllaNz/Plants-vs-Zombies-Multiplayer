// Tile.cs (versão para colocar ZUMBIS)
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isOccupied = false;

    private void OnMouseDown()
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
    // ... resto do script
}