using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject current_plant;
    public Sprite current_plant_sprite;
    public Transform tiles;
    public LayerMask tile_layer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, tile_layer);

        if(hit.collider != null)
        {
            tile t = hit.collider.GetComponent<tile>();
            if (t != null && Input.GetMouseButtonDown(0))
            {
                t.On_click();
            }
        }
    }
}
