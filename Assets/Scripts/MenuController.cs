using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    public GameObject settingsPanel;

    public void Jogar() {
        Debug.Log("Iniciando o jogo");
        SceneManager.LoadScene("Lobby");     
    }

    public void AbrirConfiguracoes() {
        Debug.Log("Abrindo configuracoes");
        settingsPanel.SetActive(true);
    }

    public void FecharConfiguracoes() {
        Debug.Log("Fechando configuracoes");
        settingsPanel.SetActive(false);
    }

    public void SairJogo() {
        Debug.Log("Jogo fechado.");
        Application.Quit();
    }
}