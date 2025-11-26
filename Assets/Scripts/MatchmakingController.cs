using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;

public class MatchmakingController : MonoBehaviourPunCallbacks
{
    // --- UI Elements - Configuração Final (TelaSala) ---
    [Header("UI Elements - Seleção de Partida")]
    public TMP_Text player1StatusText; 
    public TMP_Text player2StatusText; 
    
    // --- UI Elements - Pré-Sala ---
    [Header("UI Elements - Pré-Sala (Time/Mapa)")]
    // Estes botões devem estar na telaPreSala
    public Button preRoomZombieTeamButton;
    public Button preRoomPlantTeamButton;
    public Button preRoomMap1Button; // Opcional, para o primeiro mapa
    public Button preRoomMap2Button; // Opcional, para o segundo mapa
    public GameObject teamSelectionUI;
    public GameObject mapSelectionUI;

    // --- UI Elements - Chat ---
    [Header("UI Elements - Chat")]
    private UIControllerPhoton _uiController;
    private string _localPlayerTeam = "None";
    private ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();
    
    // Constantes para as chaves das propriedades da sala
    private const string MAP_KEY = "SelectedMap";
    private const string TIME_KEY = "MatchDuration";
    private const string P1_TEAM_KEY = "Player1Team";
    private const string P2_TEAM_KEY = "Player2Team";
    private const string P1_READY_KEY = "Player1Ready";
    private const string P2_READY_KEY = "Player2Ready";
    private const string P1_DECK_KEY = "Player1Deck"; 
    private const string P2_DECK_KEY = "Player2Deck"; 
    private void Start()
    {
        // Garante que o componente tem um PhotonView para RPCs (Chat)
        if (GetComponent<PhotonView>() == null)
        {
            gameObject.AddComponent<PhotonView>();
        }
    }

    // --- Etapa 1: Pré-Sala (Seleção de Time e Mapa Inicial) ---
    public void InitializePreRoomConfiguration(UIControllerPhoton controller)
    {
        _uiController = controller;

        // Reseta listeners para evitar duplicação
        preRoomPlantTeamButton.onClick.RemoveAllListeners();
        preRoomZombieTeamButton.onClick.RemoveAllListeners();
        
        // 1. Configura visibilidade inicial: Apenas Time
        if (teamSelectionUI != null) teamSelectionUI.SetActive(true);
        if (mapSelectionUI != null) mapSelectionUI.SetActive(false); // <--- ESCONDE OS MAPAS INICIALMENTE

        // 2. Seleção de Time - Apenas o Master Client pode fazer a 1ª seleção
        if (PhotonNetwork.IsMasterClient)
        {
            preRoomPlantTeamButton.onClick.AddListener(() => OnMasterClientSelectsTeam("Planta"));
            preRoomZombieTeamButton.onClick.AddListener(() => OnMasterClientSelectsTeam("Zumbi"));
        }
    }

    private void OnMasterClientSelectsTeam(string selectedTeam)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        // Atribui times: J1 = escolhido; J2 = restante
        _customProperties[P1_TEAM_KEY] = selectedTeam;
        _customProperties[P2_TEAM_KEY] = (selectedTeam == "Planta") ? "Zumbi" : "Planta";
        
        // Define o time local para que o J1 saiba o seu deck
        _localPlayerTeam = selectedTeam;
        
        // Envia as propriedades atualizadas
        PhotonNetwork.CurrentRoom.SetCustomProperties(_customProperties);
        Debug.Log($"Master Client escolheu: {selectedTeam}. Indo para o Deck.");

        // Esconde a seleção de time e NÃO mostra mais o mapa
        if (teamSelectionUI != null) teamSelectionUI.SetActive(false);
        if (mapSelectionUI != null) mapSelectionUI.SetActive(true);

        preRoomMap1Button.onClick.RemoveAllListeners();
        preRoomMap2Button.onClick.RemoveAllListeners();
        preRoomMap1Button.onClick.AddListener(() => OnMasterClientSelectsMap("Mapa da Floresta"));
        preRoomMap2Button.onClick.AddListener(() => OnMasterClientSelectsMap("Mapa da Cidade"));
    }

    private void OnMasterClientSelectsMap(string selectedMap)
    {
        if (!PhotonNetwork.IsMasterClient) return;
        // Salva a configuração de mapa
        _customProperties[MAP_KEY] = selectedMap;
        PhotonNetwork.CurrentRoom.SetCustomProperties(_customProperties);
        Debug.Log($"Master Client escolheu o mapa: {selectedMap}. Indo para o Deck.");
        // Esconde a seleção de mapa.
        if (mapSelectionUI != null) mapSelectionUI.SetActive(false);
        // J1 vai para o Deck
        _uiController.GoToDeckScreen(_localPlayerTeam);
        // J2 também vai quando detectar a mudança
        CheckAndMovePlayer2ToDeck();
    }
    
    // --- Chamado pelo J2 quando J1 seleciona o time ---
    // Tornei público para que o UIControllerPhoton possa acioná-lo ao entrar na sala
    public void CheckAndMovePlayer2ToDeck()
    {
        // Apenas faz sentido rodar se for o J2 e tiver 2 players
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            // Verifica o time do J2 (que foi setado pelo J1)
            string p2Team = PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(P2_TEAM_KEY)? (string)PhotonNetwork.CurrentRoom.CustomProperties[P2_TEAM_KEY]: "None";
                    
            // Esta variável garante que ele não volte para o Deck se já terminou
            bool jaEstaNoDeck = _uiController.telaDeckPlanta.activeSelf || _uiController.telaDeckZombie.activeSelf;
            Debug.Log($"[J2 DEBUG] p2Team={p2Team} | jaEstaNoDeck={jaEstaNoDeck}");
            // Se o time foi atribuído E o jogador ainda não está no deck
            if (p2Team != "None" && !jaEstaNoDeck)
            {
                Debug.Log($"Player 2 detectou escolha de time ({p2Team}). Indo para o Deck.");
                _localPlayerTeam = p2Team;
                _uiController.GoToDeckScreen(p2Team);
            }
        }
    }

    // --- Etapa 2: Configuração de Propriedades (Chamado após entrar na sala) ---
    public void SetupRoomPropertiesOnJoin()
    {
        // Garante que as propriedades essenciais existam ao entrar na sala
        _customProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        if (PhotonNetwork.IsMasterClient)
        {
            if (!_customProperties.ContainsKey(MAP_KEY)) _customProperties[MAP_KEY] = "Mapa da Floresta"; 
            if (!_customProperties.ContainsKey(TIME_KEY)) _customProperties[TIME_KEY] = "5 minutos"; 
            if (!_customProperties.ContainsKey(P1_TEAM_KEY)) _customProperties[P1_TEAM_KEY] = "None"; 
            if (!_customProperties.ContainsKey(P2_TEAM_KEY)) _customProperties[P2_TEAM_KEY] = "None"; 
            if (!_customProperties.ContainsKey(P1_READY_KEY)) _customProperties[P1_READY_KEY] = false;
            if (!_customProperties.ContainsKey(P2_READY_KEY)) _customProperties[P2_READY_KEY] = false;

            PhotonNetwork.CurrentRoom.SetCustomProperties(_customProperties);
        }

        // Recupera o status local de time para usar no fluxo de telas
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            _localPlayerTeam = _customProperties.ContainsKey(P1_TEAM_KEY) ? (string)_customProperties[P1_TEAM_KEY] : "None";
        }
        else
        {
            _localPlayerTeam = _customProperties.ContainsKey(P2_TEAM_KEY) ? (string)_customProperties[P2_TEAM_KEY] : "None";
            // Se J2 e o time já foi escolhido, move para o deck imediatamente
            CheckAndMovePlayer2ToDeck(); 
        }
    }
    
    // --- Etapa 3: Configuração Final na TelaSala (Botão da Tela de Deck) ---
    
    public void FinishDeckConfiguration(List<string> selectedDeck)
    {
        // 1. Salva o deck no Room Properties
        string deckKey = PhotonNetwork.LocalPlayer.IsMasterClient ? P1_DECK_KEY : P2_DECK_KEY;
        
        // Photon aceita arrays de string para serialização
        _customProperties[deckKey] = selectedDeck.ToArray(); 
        
        // 2. Se marca como pronto automaticamente ao sair do deck
        // Se o deck é o último passo antes da TelaSala, você pode se marcar como pronto aqui:
        // Apenas mude o status local, o UpdateUIFromRoomProperties se encarregará de atualizar a UI.
        string readyKey = PhotonNetwork.LocalPlayer.IsMasterClient ? P1_READY_KEY : P2_READY_KEY;
        _customProperties[readyKey] = true;
        PhotonNetwork.CurrentRoom.SetCustomProperties(_customProperties);

        // 3. Move para a Tela Sala
        Debug.Log("Configuração de Deck finalizada. Carregando a partida.");
        _uiController.GoToLoadingScreen();
    }
    
    // --- Callbacks do Photon ---
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (propertiesThatChanged.ContainsKey(P1_TEAM_KEY) || propertiesThatChanged.ContainsKey(MAP_KEY))
            {
                CheckAndMovePlayer2ToDeck(); // Corrigido
            }
        }

        UpdatePlayerReadyStatusUI();
        CheckAllPlayersReady();
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // O Master Client pode querer inicializar as propriedades do novo jogador
        if (PhotonNetwork.IsMasterClient)
        {
            // Isso já é feito no SetupRoomPropertiesOnJoin, mas é bom garantir aqui
            _customProperties[P2_TEAM_KEY] = "None";
            _customProperties[P2_READY_KEY] = false;
            PhotonNetwork.CurrentRoom.SetCustomProperties(_customProperties);
        }        
        UpdatePlayerReadyStatusUI(); 
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);
        
        // Reseta o status do jogador que saiu (se o local for o Master Client)
        if (PhotonNetwork.IsMasterClient)
        {
            _customProperties[P2_TEAM_KEY] = "None";
            _customProperties[P2_READY_KEY] = false;
            PhotonNetwork.CurrentRoom.SetCustomProperties(_customProperties);
        }
        
        // Se o Master Client saiu, o próximo jogador assume e as telas podem resetar
        if (otherPlayer.IsMasterClient)
        {
            // Idealmente, a sala volta para o lobby ou o novo Master Client assume as rédeas
        }

        UpdatePlayerReadyStatusUI();
        
        // Se a sala fica com menos de 2 jogadores, volta para a Pré-Sala/Lobby
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            // Exemplo: Volta para a Pré-Sala para esperar
            _uiController.AtivarTela(_uiController.telaPreSala);
        }
    }

    // --- Métodos de Lógica Interna ---

    public void UpdateUIFromRoomProperties(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {

        UpdatePlayerReadyStatusUI();
    }

    public void UpdatePlayerReadyStatusUI()
    {
        // Pega as propriedades mais recentes da sala para exibir
        ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
        
        bool p1Ready = roomProps.ContainsKey(P1_READY_KEY) ? (bool)roomProps[P1_READY_KEY] : false;
        bool p2Ready = roomProps.ContainsKey(P2_READY_KEY) ? (bool)roomProps[P2_READY_KEY] : false;
        string p1Team = roomProps.ContainsKey(P1_TEAM_KEY) ? (string)roomProps[P1_TEAM_KEY] : "None";
        string p2Team = roomProps.ContainsKey(P2_TEAM_KEY) ? (string)roomProps[P2_TEAM_KEY] : "None";

        // Verifica se a lista de players é válida para evitar erros
        if (PhotonNetwork.PlayerList.Length == 0) return;

        Player masterClient = PhotonNetwork.PlayerList[0]; 
        Player otherPlayer = PhotonNetwork.PlayerList.Length > 1 ? PhotonNetwork.PlayerList[1] : null;

        // Exibe o status do Master Client (Jogador 1)
        if (player1StatusText != null)
        {
            player1StatusText.text = $"{masterClient.NickName} ({p1Team}): " + (p1Ready ? "PRONTO" : "Aguardando");
        }
        
        // Exibe o status do outro jogador (Jogador 2)
        if (player2StatusText != null)
        {
            if (otherPlayer != null)
            {
                player2StatusText.text = $"{otherPlayer.NickName} ({p2Team}): " + (p2Ready ? "PRONTO" : "Aguardando");
            }
            else
            {
                player2StatusText.text = "Aguardando Jogador...";
            }
        }
    }

    void CheckAllPlayersReady()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            ExitGames.Client.Photon.Hashtable roomProps = PhotonNetwork.CurrentRoom.CustomProperties;
            bool p1Ready = roomProps.ContainsKey(P1_READY_KEY) ? (bool)roomProps[P1_READY_KEY] : false;
            bool p2Ready = roomProps.ContainsKey(P2_READY_KEY) ? (bool)roomProps[P2_READY_KEY] : false;

            if (p1Ready && p2Ready)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    Debug.Log("Ambos os jogadores estão prontos! Iniciando a partida...");
                    // Carrega a cena do jogo - deve ser a cena com o número de build index ou nome correto
                    PhotonNetwork.LoadLevel("GameScene"); // Substitua "GameScene" pelo nome da sua cena de jogo
                }
            }
        }
    }
}