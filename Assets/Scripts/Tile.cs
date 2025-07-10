using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool temzombie;
    private bool isOccupied = false; // Controla se o slot j� tem uma planta

    // OnMouseDown � chamado quando o mouse � clicado sobre um Collider
    private void OnMouseDown()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void On_click()
    {
        if (!temzombie)
        // 1. Pergunta ao gerenciador qual prefab est� selecionado
        GameObject plantPrefab = PlantingManager.Instance.GetSelectedPlantPrefab();

        // 2. Verifica se um prefab v�lido foi retornado e se o slot est� livre
        if (plantPrefab != null && !isOccupied)
        {
            // 3. Instancia a planta na posi��o deste slot
            Instantiate(plantPrefab, transform.position, Quaternion.identity);

            // 4. Marca o slot como ocupado
            isOccupied = true;

            // 5. Informa ao gerenciador que o plantio foi conclu�do
            // Isso vai consumir os s�is e deselecionar a planta.
            PlantingManager.Instance.PlantWasPlaced();

            Debug.Log("Planta colocada em " + gameObject.name);
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

    void Awake()
    {
        // Adiciona BoxCollider2D s� se ainda n�o existir
        if (!TryGetComponent<BoxCollider2D>(out _))
        {
            BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            col.size = Vector2.one;   // 1�1 unidade
            col.offset = Vector2.zero;
            col.isTrigger = true;     // se n�o precisar de f�sica
        }
    }
}
