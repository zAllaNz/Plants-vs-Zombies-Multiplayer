using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class UIController : MonoBehaviour
{
    [Header("Telas")]
    public GameObject telaInicial;
    public GameObject telaHostEsperando;
    public GameObject telaClientIP;
    public GameObject telaClientConectando;
    public GameObject telaSucesso;

    [Header("Botões")]
    public Button hostButton;
    public Button clientButton;
    public Button conectarButton;

    [Header("Campo de IP")]
    public TMP_InputField ipInputField;

    private void Start()
    {
        // Começa na tela inicial
        AtivarTela(telaInicial);

        // Atribui eventos dos botões
        hostButton.onClick.AddListener(SelecionarHost);
        clientButton.onClick.AddListener(SelecionarClient);
        conectarButton.onClick.AddListener(ConectarClient);

        // Conecta eventos do NetworkManager
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted += OnHostStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnDestroy()
    {
        // Remove os eventos (boa prática)
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted -= OnHostStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    void AtivarTela(GameObject telaParaAtivar)
    {
        telaInicial.SetActive(false);
        telaHostEsperando.SetActive(false);
        telaClientIP.SetActive(false);
        telaClientConectando.SetActive(false);
        telaSucesso.SetActive(false);

        telaParaAtivar.SetActive(true);
    }

    void SelecionarHost()
    {
        bool sucesso = NetworkManager.Singleton.StartHost();
        Debug.Log(NetworkManager.Singleton.StartHost());

        if (sucesso)
        {
            Debug.Log("Host iniciado. Aguardando jogador...");
            AtivarTela(telaHostEsperando);
        }
        else
        {
            Debug.LogWarning("Falha ao iniciar como Host.");
            AtivarTela(telaInicial);
        }
    }

    void SelecionarClient()
    {
        AtivarTela(telaClientIP);
    }

    void ConectarClient()
    {
        string ip = ipInputField.text.Trim();

        if (string.IsNullOrEmpty(ip))
        {
            Debug.LogWarning("IP inválido ou vazio.");
            return;
        }

        var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
        transport.SetConnectionData(ip, 7777); // Porta padrão

        bool sucesso = NetworkManager.Singleton.StartClient();

        if (sucesso)
        {
            Debug.Log("Tentando conectar ao host...");
            AtivarTela(telaClientConectando);
        }
        else
        {
            Debug.LogWarning("Falha ao iniciar como Client.");
            AtivarTela(telaClientIP);
        }
    }

    void OnHostStarted()
    {
        Debug.Log("Host iniciado com sucesso.");
        // Aguardamos o jogador 2 se conectar — nada a fazer aqui
    }

    void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            Debug.Log("Cliente conectado com sucesso.");
            AtivarTela(telaSucesso);
        }
        else
        {
            Debug.Log($"Outro jogador se conectou (ID {clientId})");
            if (NetworkManager.Singleton.IsHost)
            {
                AtivarTela(telaSucesso);
            }
        }
    }
}
