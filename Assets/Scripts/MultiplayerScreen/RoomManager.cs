using Photon.Pun;
using UnityEngine;

public class RoomManager: MonoBehaviourPunCallbacks
{
    public PhotonView player;
    public PhotonView player2;
    public Transform spawnPoint;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to master server");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("test", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
        GameObject _player = null;

        if (player == null)
        {
            Debug.LogError("El prefab del jugador (player) no está asignado.");
            return;
        }
        if (player2 == null)
        {
            Debug.LogError("El prefab del jugador (player2) no está asignado.");
            return;
        }
        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint no está asignado en el Inspector.");
            return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
             _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, spawnPoint.rotation);
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            _player = PhotonNetwork.Instantiate(player2.name, spawnPoint.position, spawnPoint.rotation);

        }

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