using UnityEngine;
using TMPro;
using Unity.Netcode;
using System.Collections;

public class LobbyUI : MonoBehaviour
{
    public TMP_Text joinCodeText;
    public TMP_InputField joinCodeInput;
    public GameObject roleSelectionPanel;

    public async void CreateRoom()
    {
        joinCodeText.text = "Criando...";

        string code = await RelayManager.Instance.CreateRoom();
        joinCodeText.text = "Código: " + code;

        StartCoroutine(WaitForHost());
    }

    public async void JoinRoom()
    {
        if (joinCodeInput.text.Length < 6)
            return;

        await RelayManager.Instance.JoinRoom(joinCodeInput.text);
    }

    public void SelectPlants()
    {
        Debug.Log("[LobbyUI] Botão PLANTS clicado");

        SessionData.Instance.SetHostRoleServerRpc(PlayerRole.Plants);
        roleSelectionPanel.SetActive(false);
    }

    public void SelectZombies()
    {
        Debug.Log("[LobbyUI] Botão ZOMBIES clicado");

        SessionData.Instance.SetHostRoleServerRpc(PlayerRole.Zombies);
        roleSelectionPanel.SetActive(false);
    }
    private void Start()
    {
        roleSelectionPanel.SetActive(false);
    }
    
    private IEnumerator WaitForHost()
    {
        while (!NetworkManager.Singleton.IsHost)
            yield return null;

        Debug.Log("[LobbyUI] Host confirmado, mostrando seleção de role");
        roleSelectionPanel.SetActive(true);
    }
}
