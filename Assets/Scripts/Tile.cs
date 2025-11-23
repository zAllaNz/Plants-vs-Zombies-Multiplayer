
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isOccupied = false;

    // método para ver a variavél
    public bool estaOcupado
    {
        get { return isOccupied; }
    }

 

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

        // logica dos zombies (integrando com as plantas)

       // GameObject zombiePrefab = ZombieManager.Instance.GetSelectedZombiePrefab();

        //if (zombiePrefab != null)
        //{
          //  Instantiate(zombiePrefab, transform.position, Quaternion.identity);
            //ZombieManager.Instance.ZombieWasPlaced();

            //return;
        //}

    }
    }

  