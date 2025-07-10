using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;


public class zombie_slot : MonoBehaviour
{
    public Sprite zombie_sprite;

    public GameObject zombie_object;

    public Image Icon;

    public int Price;
    public TextMeshProUGUI price_Text;

    public GameManager gms;

    public void Start()
    {
        gms = GameObject.Find("GameManager").GetComponent<GameManager>();
        GetComponent<Button>().onClick.AddListener(comprar_zombie);

    }

    private void comprar_zombie()
    {

        gms.comprar_zombie(zombie_object, zombie_sprite);

    }


    private void OnValidate()
    {
        if (zombie_sprite)
        {
            Icon.enabled = true;
            Icon.sprite = zombie_sprite;
            price_Text.text = Price.ToString();
        }
        else
        {
            Icon.enabled = false;
        }


        
    }

};
