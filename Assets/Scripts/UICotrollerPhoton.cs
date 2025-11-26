using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class UIControllerPhoton : MonoBehaviourPunCallbacks
{
    [Header("Telas")]
    public GameObject telaLogin;
    public GameObject telaLobby;
    public GameObject telaCarregamento;
    public GameObject telaPreSala;
    public GameObject telaDeckPlanta;
    public GameObject telaDeckZombie;
    public MatchmakingController matchmakingController;

    [Header("Status Players (para Matchmaking)")]
    public TMP_Text matchmakingPlayer1Text; 
    public TMP_Text matchmakingPlayer2Text;

    [Header("Animação de Carregamento")]
    public Animator loadingFlowerAnimator;
    
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
        if (loadingFlowerAnimator == null)
        {
        Debug.LogWarning("Animator da flor de carregamento não está atribuído!");
        }
    }

    public void AtivarTela(GameObject telaParaAtivar)
    {
        telaLogin.SetActive(telaParaAtivar == telaLogin);
        telaLobby.SetActive(telaParaAtivar == telaLobby);
        telaPreSala.SetActive(telaParaAtivar == telaPreSala);
        telaCarregamento.SetActive(telaParaAtivar == telaCarregamento);
        telaDeckPlanta.SetActive(telaParaAtivar == telaDeckPlanta);
        telaDeckZombie.SetActive(telaParaAtivar == telaDeckZombie);
         if (telaParaAtivar == telaCarregamento && loadingFlowerAnimator != null)
        {
        loadingFlowerAnimator.Play("TelaCarregamento"); // Substitua pelo nome real da sua animação
        Debug.Log("Iniciando animação de carregamento");
        }
        if (matchmakingController != null)
        {
            bool ativarMatchmaking = (
                telaParaAtivar == telaPreSala ||
                telaParaAtivar == telaDeckPlanta ||
                telaParaAtivar == telaDeckZombie ||
                telaParaAtivar == telaCarregamento
                );
            matchmakingController.gameObject.SetActive(ativarMatchmaking);
        }
    }

    private void TentarEntrarNoLobby()
    {
        // Define o Nickname
        if (!string.IsNullOrEmpty(ipInputField.text))
        {
            PhotonNetwork.NickName = ipInputField.text;
        }
        else
        {
            PhotonNetwork.NickName = "Player" + Random.Range(1000, 9999);
        }

        tentativaDeLogin = true;
        Debug.Log("Tentando entrar no lobby, ativando tela de carregamento");
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

        if (matchmakingController != null)
        {
            //Antes de qualquer coisa: garantir que o MatchmakingController tenha os textos da UI
            if (matchmakingPlayer1Text != null)
                matchmakingController.player1StatusText = matchmakingPlayer1Text;

            if (matchmakingPlayer2Text != null)
                matchmakingController.player2StatusText = matchmakingPlayer2Text;

            //Atualiza/Carrega propriedades da sala
            matchmakingController.SetupRoomPropertiesOnJoin();

            //Se for o Jogador 1 (Master Client)
            if (PhotonNetwork.IsMasterClient)
            {
                // Vai para a pré-sala para configurar a partida
                AtivarTela(telaPreSala);
                matchmakingController.InitializePreRoomConfiguration(this);
            }
            else
            {
                //Jogador 2 começa na tela de carregamento
                AtivarTela(telaCarregamento);
                // Aguarda 0.2s e só então verifica o deck
                StartCoroutine(DelayCheckForDeck());
            }
        }
    }

    
    // Controla a navegação para a tela de Deck
    public void GoToDeckScreen(string team)
    {
        if (team == "Planta")
        {
            AtivarTela(telaDeckPlanta);
        }
        else if (team == "Zumbi")
        {
            AtivarTela(telaDeckZombie);
        }
    }    
    public void GoToLoadingScreen()
    {
        AtivarTela(telaCarregamento);
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

    private IEnumerator DelayCheckForDeck()
    {
        yield return new WaitForSeconds(0.2f);
        matchmakingController.CheckAndMovePlayer2ToDeck();
    }
}