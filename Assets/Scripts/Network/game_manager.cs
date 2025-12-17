using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class GameManagerNew : NetworkBehaviour
{
    public GameObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnSceneLoaded;
    }

    private void OnSceneLoaded(
        string sceneName,
        UnityEngine.SceneManagement.LoadSceneMode mode,
        System.Collections.Generic.List<ulong> clientsCompleted,
        System.Collections.Generic.List<ulong> clientsTimedOut)
    {
        if (sceneName != "Gameplay") return;

        StartCoroutine(SpawnPlayers());
    }

    private IEnumerator SpawnPlayers()
    {
        yield return null;

        PlayerRole hostRole = SessionData.Instance.HostRole.Value;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            GameObject player = Instantiate(playerPrefab);
            NetworkObject netObj = player.GetComponent<NetworkObject>();
            netObj.SpawnAsPlayerObject(clientId);

            PlayerController data = player.GetComponent<PlayerController>();

            if (clientId == 0)
                data.Role.Value = hostRole;
            else
                data.Role.Value =
                    hostRole == PlayerRole.Plants
                        ? PlayerRole.Zombies
                        : PlayerRole.Plants;
        }
    }
}
