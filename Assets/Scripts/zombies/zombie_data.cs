
using UnityEngine;

// A linha abaixo permite que eu crie novos "ZombieData" pelo menu do Unity
// (Assets -> Create -> Gameplay -> New Zombie Data)
[CreateAssetMenu(fileName = "New Zombie Data", menuName = "Gameplay/New Zombie Data")]
public class ZombieData : ScriptableObject
{
    public string zombieName;
    public GameObject zombiePrefab; // O prefab do zumbi que será instanciado
    public int brainCost;
    public Sprite icon;// O custo em "cérebros" 
}