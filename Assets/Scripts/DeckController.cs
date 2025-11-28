// DeckController.cs (Novo Arquivo)

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DeckController : MonoBehaviour
{
    [Header("Configuração de Cartas")]
    public Button[] cardButtons; 
    public Color selectedColor = Color.green;
    public Color unselectedColor = Color.white;
    public const int MaxCards = 5;

    [Header("UI e Conexão")]
    public MatchmakingController matchmakingController; // Conexão com o Matchmaking
    public Button readyButton; // Botão "Estou pronto"
    public TMP_Text cardCountText; // Opcional: para mostrar X/5 cartas selecionadas

    private List<string> selectedDeck = new List<string>();
    
    private void Start()
    {
        InitializeDeckUI();
    }

    public void InitializeDeckUI()
    {
        selectedDeck.Clear();
        readyButton.interactable = false;
        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(OnDeckReady);
        
        if (cardCountText != null) cardCountText.text = $"Cartas Selecionadas: 0/{MaxCards}";

        for (int i = 0; i < cardButtons.Length; i++)
        {
            Button cardButton = cardButtons[i];
            // O nome do GameObject do botão pode ser usado como o nome da carta
            string cardName = cardButton.name; 
            
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() => OnCardClicked(cardButton, cardName));
            
            // Reseta a cor visualmente
            SetButtonColor(cardButton, unselectedColor);
        }
    }

    private void OnCardClicked(Button button, string cardName)
    {
        if (selectedDeck.Contains(cardName))
        {
            // Desselecionar: Remover do deck e mudar a cor para a normal
            selectedDeck.Remove(cardName);
            SetButtonColor(button, unselectedColor);
        }
        else
        {
            // Selecionar: Se o limite não foi atingido
            if (selectedDeck.Count < MaxCards)
            {
                selectedDeck.Add(cardName);
                SetButtonColor(button, selectedColor);
            }
            else
            {
                // Opcional: feedback para o usuário de que o limite foi atingido
                Debug.Log("Limite de 5 cartas atingido.");
            }
        }
        
        UpdateReadyButtonState();
    }

    private void SetButtonColor(Button button, Color color)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
        }
    }

    private void UpdateReadyButtonState()
    {
        bool isReady = selectedDeck.Count == MaxCards;
        readyButton.interactable = isReady;
        
        if (cardCountText != null) cardCountText.text = $"Cartas Selecionadas: {selectedDeck.Count}/{MaxCards}";
    }

    private void OnDeckReady()
    {
        Debug.Log("Deck Final Selecionado: " + string.Join(", ", selectedDeck));
        // Chama o método no MatchmakingController para salvar o deck e avançar
        matchmakingController.FinishDeckConfiguration(selectedDeck);
    }
}