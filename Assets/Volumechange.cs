using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Volumechange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Dropdown graphicsDropdown;
    public Slider masterVolume, musicVolume, sfxVolume;
    public AudioMixer mainAudioMixer;

    public void changeGraphicsQuality() {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }

    public void ChangeMasterVolume() {
        mainAudioMixer.SetFloat("MasterVolume", masterVolume.value);
    }

    public void ChangeMusicVolume() {
        mainAudioMixer.SetFloat("MusicVolume", musicVolume.value);
    }

    public void ChangeSFXVolume() {
        mainAudioMixer.SetFloat("SFXVolume", sfxVolume.value);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
