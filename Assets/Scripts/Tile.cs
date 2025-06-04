using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class tile : MonoBehaviour
{
    public bool has_plant;

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
        if (!has_plant)
        {
            Debug.Log("Cliquei neste tile: " + gameObject.name);
        }
    }
}
