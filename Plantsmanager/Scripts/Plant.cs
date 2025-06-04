using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public GameObject selectedPlantPrefab; // Prefab da planta selecionada
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique esquerdo
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();

                // Se clicou em um tile válido e ele não tem planta
                if (tile != null && !tile.hasPlant && selectedPlantPrefab != null)
                {
                    // Instancia a planta na posição do tile
                    GameObject newPlant = Instantiate(selectedPlantPrefab, tile.transform.position, Quaternion.identity);
                    
                    // Atualiza o estado do tile
                    tile.hasPlant = true;
                    tile.currentPlant = newPlant;
                }
            }
        }
    }

    // Método para definir a planta selecionada externamente
    public void SelectPlant(GameObject plantPrefab)
    {
        selectedPlantPrefab = plantPrefab;
    }
}

