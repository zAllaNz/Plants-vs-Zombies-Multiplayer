using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool temzombie;
    private bool isOccupied = false; // Controla se o slot j� tem uma planta

    // OnMouseDown � chamado quando o mouse � clicado sobre um Collider
    private void OnMouseDown()
    {
        GameObject plantPrefab = PlantingManager.Instance.GetSelectedPlantPrefab();
        if (plantPrefab != null && !isOccupied)
        {
            Instantiate(plantPrefab, transform.position, Quaternion.identity);
            isOccupied = true;
            PlantingManager.Instance.PlantWasPlaced();
        }
        else if (isOccupied)
        {
            Debug.Log("Este slot j� est� ocupado!");
        }
        else
        {
            Debug.Log("Nenhuma planta selecionada para plantar.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void On_click()
    {

    }

    void Awake()
    {
        if (!TryGetComponent<BoxCollider2D>(out _))
        {
            BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            col.size = Vector2.one;
            col.offset = Vector2.zero;
            col.isTrigger = true;
        }
    }
}
