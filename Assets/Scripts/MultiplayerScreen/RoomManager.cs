using Photon.Pun;
using UnityEngine;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public PhotonView player;
    public Transform spawnPoint;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master server");
        Debug.Log(RoomSelector.roomName);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log(RoomSelector.roomName);
        PhotonNetwork.JoinOrCreateRoom(RoomSelector.roomName, null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.LogError("Joined room" + PhotonNetwork.CurrentRoom.Name + " PLAYERS: "+ PhotonNetwork.CurrentRoom.Players);

        if (player == null)
        {
            Debug.LogError("El prefab del jugador (player) no está asignado.");
            return;
        }
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint no está asignado en el Inspector.");
            return;
        }

        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, spawnPoint.rotation);

        PlayerSetup setup = _player.GetComponent<PlayerSetup>();
        if (setup != null)
        {
            setup.IsLocalPlayer();
        }
        else
        {
            Debug.LogError("El prefab del jugador no tiene el script PlayerSetup.");
        }
    }


}
