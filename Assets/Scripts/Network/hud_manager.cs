using Unity.Netcode;
using UnityEngine;

public class HUDManager : NetworkBehaviour
{
    public GameObject plantsHUD;
    public GameObject zombiesHUD;

    public override void OnNetworkSpawn()
    {
        if (!IsClient) return;

        TrySetupHUD();
    }
    private void TrySetupHUD()
    {
        if (NetworkManager.Singleton.LocalClient.PlayerObject == null)
        {
            Debug.Log("[HUD] Player local ainda não existe, aguardando...");
            Invoke(nameof(TrySetupHUD), 0.1f);
            return;
        }

        PlayerController localPlayer =
            NetworkManager.Singleton.LocalClient.PlayerObject
            .GetComponent<PlayerController>();

        ApplyHUD(localPlayer.Role.Value);

        // ouvir mudanças futuras
        localPlayer.Role.OnValueChanged += OnRoleChanged;
    }
    private void ApplyHUD(PlayerRole role)
    {
        plantsHUD.SetActive(role == PlayerRole.Plants);
        zombiesHUD.SetActive(role == PlayerRole.Zombies);

        Debug.Log($"[HUD] HUD ativada para {role}");
    }
    private void OnRoleChanged(PlayerRole oldRole, PlayerRole newRole)
    {
        ApplyHUD(newRole);
    }
    public override void OnDestroy()
    {
        if (NetworkManager.Singleton?.LocalClient?.PlayerObject == null) return;

        var player =
            NetworkManager.Singleton.LocalClient.PlayerObject
            .GetComponent<PlayerController>();

        if (player != null)
            player.Role.OnValueChanged -= OnRoleChanged;
    }
}
