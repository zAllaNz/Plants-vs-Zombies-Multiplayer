using UnityEngine;
using TMPro;

public class SunPoints : MonoBehaviour
{
    public TextMeshProUGUI sunText;
    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Encontra o GameManager na cena para gerenciar os sóis
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        sunText.text = gameManager.currentSun.ToString();
    }
}
