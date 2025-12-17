using Unity.Netcode;
using UnityEngine;

public class SessionData : NetworkBehaviour
{
    public static SessionData Instance;

    public NetworkVariable<PlayerRole> HostRole =
        new NetworkVariable<PlayerRole>(
            PlayerRole.None,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log($"[SessionData] Awake - HostRole inicial = {HostRole.Value}");
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"[SessionData] OnNetworkSpawn - IsServer={IsServer} HostRole={HostRole.Value}");
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetHostRoleServerRpc(PlayerRole role)
    {
        Debug.Log($"[SessionData] ServerRpc chamado -> role = {role}");
        HostRole.Value = role;
        Debug.Log($"[SessionData] HostRole agora = {HostRole.Value}");
    }
}