using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantCardUI : MonoBehaviour
{
    public PlantData plantToSelect;

    public Image plantIcon;
    public TextMeshProUGUI costText;

    private Button cardButton;

    void Start()
    {
        // Configura o visual do card com base nos dados
        plantIcon.sprite = plantToSelect.cardSprite;
        costText.text = plantToSelect.sunCost.ToString();

        // Pega o componente Button e adiciona um listener para o clique
        cardButton = GetComponent<Button>();
        cardButton.onClick.AddListener(OnCardClicked);
    }

    // Este método é chamado quando o botão é clicado
    void OnCardClicked()
    {
        // Informa ao PlantingManager qual planta foi selecionada
        PlantingManager.Instance.SelectPlant(plantToSelect);
    }
}