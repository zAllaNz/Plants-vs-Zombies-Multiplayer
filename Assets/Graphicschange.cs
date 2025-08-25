using UnityEngine;
using TMPro;

public class Graphicschange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TMP_Dropdown graphicsDropdown;

    public void changeGraphicsQuality() {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
