using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Tile : MonoBehaviour
{
    public bool temzombie;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void On_click()
    {
        if (!temzombie)
        {
            Debug.Log("Cliquei neste tile: " + gameObject.name);
        }
    }

    void Awake()
    {
        // Adiciona BoxCollider2D só se ainda não existir
        if (!TryGetComponent<BoxCollider2D>(out _))
        {
            BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            col.size = Vector2.one;   // 1×1 unidade
            col.offset = Vector2.zero;
            col.isTrigger = true;     // se não precisar de física
        }
    }
}
