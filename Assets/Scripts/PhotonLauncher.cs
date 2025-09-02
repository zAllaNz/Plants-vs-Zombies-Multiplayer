using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); // Conecta ao servidor da Photon
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado ao servidor Photon");
        PhotonNetwork.JoinLobby(); // Entra no lobby para ver/entrar em salas
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Entrou no lobby");
    }
}
