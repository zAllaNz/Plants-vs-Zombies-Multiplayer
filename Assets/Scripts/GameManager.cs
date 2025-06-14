using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject current_zombi;

    public Sprite current_zombie_sprite;

    public Transform tiles;

    public LayerMask tileMask;

    public void comprar_zombie(GameObject zombie, Sprite sprite)
    {
        current_zombi = zombie;
        current_zombie_sprite = sprite;
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition), 
            Vector2.zero, 
            Mathf.Infinity,
            tileMask);

        foreach (Transform tile in tiles)
            tile.GetComponent<SpriteRenderer>().enabled = false;

        if(hit.collider && current_zombi)
        {
            hit.collider.GetComponent<SpriteRenderer>().sprite = current_zombie_sprite;
            hit.collider.GetComponent<SpriteRenderer>().enabled = true;

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(current_zombi, hit.collider.transform.position, Quaternion.identity);
                current_zombi = null;
                current_zombie_sprite = null;
            }
        }




    }
};