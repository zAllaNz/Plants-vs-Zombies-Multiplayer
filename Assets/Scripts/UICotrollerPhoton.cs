using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;

public class UIControllerPhoton : MonoBehaviourPunCallbacks
{
    [Header("Telas")]
    public GameObject telaLogin;
    public GameObject telaLobby;
    public GameObject telaSala;
    public GameObject telaCarregamento;

    [Header("Campos de Login")]
    public TMP_InputField ipInputField;
    public Button loginButton;

    [Header("Salas e UI")]
    public Button[] roomButtons;
    public Color corSalaDisponivel = Color.white;
    public Color corSalaCheia = Color.red;

    private const int maxPlayersPerRoom = 2;
    private readonly string[] roomNames = { "Sala1", "Sala2", "Sala3", "Sala4", "Sala5" };
    private List<RoomInfo> listaSalasPhoton = new List<RoomInfo>();
    private bool tentativaDeLogin = false;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        AtivarTela(telaLogin);
        loginButton.interactable = false;
        loginButton.onClick.AddListener(TentarEntrarNoLobby);

        for (int i = 0; i < roomButtons.Length; i++)
        {
            int index = i;
            roomButtons[i].onClick.AddListener(() => TentarEntrarNaSala(roomNames[index]));
        }

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void AtivarTela(GameObject telaParaAtivar)
    {
        telaLogin.SetActive(telaParaAtivar == telaLogin);
        telaLobby.SetActive(telaParaAtivar == telaLobby);
        telaSala.SetActive(telaParaAtivar == telaSala);
        telaCarregamento.SetActive(telaParaAtivar == telaCarregamento);
    }

    private void TentarEntrarNoLobby()
    {
        tentativaDeLogin = true;
        AtivarTela(telaCarregamento);
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnConnectedToMaster()
    {
        loginButton.interactable = true;
    }

    public override void OnJoinedLobby()
    {
        if (tentativaDeLogin)
        {
            AtivarTela(telaLobby);
        }
    }

    // O callback mais importante para a sua necessidade!
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        listaSalasPhoton = roomList;
        Debug.Log("Lista de salas atualizada.");
        AtualizarInterfaceSalas();
    }

    private void AtualizarInterfaceSalas()
    {
        for (int i = 0; i < roomNames.Length; i++)
        {
            RoomInfo roomInfo = listaSalasPhoton.Find(r => r.Name == roomNames[i] && !r.RemovedFromList);

            int playerCount = 0;
            bool salaExiste = roomInfo != null;
            bool salaEstaCheia = false;

            if (salaExiste)
            {
                playerCount = roomInfo.PlayerCount;
                salaEstaCheia = playerCount >= maxPlayersPerRoom;
            }

            // 1. Atualizar o texto do botão com a contagem de jogadores
            TMP_Text buttonText = roomButtons[i].GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = $"{roomNames[i]}({playerCount}/{maxPlayersPerRoom})";
            }

            // 2. Mudar a cor do botão para sinalizar que a sala está cheia
            Image buttonImage = roomButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = salaEstaCheia ? corSalaCheia : corSalaDisponivel;
            }

            // 3. Trancar a sala (desativar o botão)
            roomButtons[i].interactable = !salaEstaCheia;
        }
    }

    private void TentarEntrarNaSala(string nomeSala)
    {
        AtivarTela(telaCarregamento);
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = (byte)maxPlayersPerRoom,
            IsVisible = true,
            IsOpen = true
        };
        PhotonNetwork.JoinOrCreateRoom(nomeSala, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Entrou na sala " + PhotonNetwork.CurrentRoom.Name);
        tentativaDeLogin = false;
        AtivarTela(telaSala);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Falha ao entrar na sala: " + message);
        AtivarTela(telaLobby);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Desconectado por: {cause}");
        tentativaDeLogin = false;
        AtivarTela(telaLogin);
        loginButton.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }
}