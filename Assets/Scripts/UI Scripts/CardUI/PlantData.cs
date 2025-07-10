using UnityEngine;

[CreateAssetMenu(fileName = "New Plant Data", menuName = "Game/Plant Data")]
public class PlantData : ScriptableObject
{
    [Header("Informações Gerais")]
    public string plantName;
    public GameObject plantPrefab; // O prefab da planta que será instanciado no jogo

    [Header("UI do Card")]
    public Sprite cardSprite; // O ícone que aparecerá no card

    [Header("Custo")]
    public int sunCost;
}
