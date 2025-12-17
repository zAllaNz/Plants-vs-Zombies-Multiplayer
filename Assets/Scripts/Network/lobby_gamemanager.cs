using Unity.Netcode;
using UnityEngine;

public class LobbyGameManager : NetworkBehaviour
{
    public NetworkVariable<PlayerRole> HostRole =
        new NetworkVariable<PlayerRole>(PlayerRole.None);

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == 2)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(
                "Gameplay",
                UnityEngine.SceneManagement.LoadSceneMode.Single
            );
        }
    }
}
