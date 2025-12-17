using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float speed = 5f;
    public NetworkVariable<PlayerRole> Role =
        new NetworkVariable<PlayerRole>(PlayerRole.None);

    public override void OnNetworkSpawn()
    {
        Debug.Log($"LocalClientId: {NetworkManager.Singleton.LocalClientId}");
        Debug.Log($"OwnerClientId: {OwnerClientId}");
        Debug.Log($"IsOwner: {IsOwner}");
        Role.OnValueChanged += (oldRole, newRole) =>
        {
            Debug.Log($"Role mudou para: {newRole}");
        };
    }

    void Update()
    {
        if (!IsOwner) return;

        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) dir.y += 1;
        if (Input.GetKey(KeyCode.S)) dir.y -= 1;
        if (Input.GetKey(KeyCode.A)) dir.x -= 1;
        if (Input.GetKey(KeyCode.D)) dir.x += 1;

        transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
