using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using System.Threading.Tasks;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public async Task<string> CreateRoom()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetRelayServerData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData
        );

        NetworkManager.Singleton.StartHost();
        Debug.Log("Host iniciado");

        return joinCode;
    }

    public async Task JoinRoom(string joinCode)
    {
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetRelayServerData(
            allocation.RelayServer.IpV4,
            (ushort)allocation.RelayServer.Port,
            allocation.AllocationIdBytes,
            allocation.Key,
            allocation.ConnectionData,
            allocation.HostConnectionData
        );

        NetworkManager.Singleton.StartClient();
        Debug.Log("Cliente conectado");
    }
}
