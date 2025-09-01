using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ResolutionControler : MonoBehaviour {
    public TMP_Dropdown ResDropDown;
    public Toggle FullScreenToggle;

    Resolution[] AllResolutions;
    List<Resolution> SelectedResolutionList = new List<Resolution>();

    void Start() {
        // Define fullscreen como padrão
        FullScreenToggle.isOn = Screen.fullScreen;

        // Pega todas as resoluções suportadas
        AllResolutions = Screen.resolutions;

        List<string> resolutionStringList = new List<string>();

        foreach (Resolution resolution in AllResolutions) {
            string newResolution = resolution.width + " x " + resolution.height;

            if (!resolutionStringList.Contains(newResolution)) {
                resolutionStringList.Add(newResolution);
                SelectedResolutionList.Add(resolution);
            }
        }

        // Preenche o Dropdown
        ResDropDown.ClearOptions();
        ResDropDown.AddOptions(resolutionStringList);

        // Detecta mudanças
        ResDropDown.onValueChanged.AddListener(SetResolution);
        FullScreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    void SetResolution(int index) {
        Resolution resolution = SelectedResolutionList[index];
        Screen.SetResolution(resolution.width, resolution.height, FullScreenToggle.isOn);
    }

    void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;

        // Atualiza para aplicar resolução atual no modo alterado
        SetResolution(ResDropDown.value);
    }
}
