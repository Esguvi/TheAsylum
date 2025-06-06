using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class RoomSelector : MonoBehaviourPunCallbacks
{
    public static string roomName;
    public TextMeshProUGUI roomInput;

    private List<RoomInfo> cachedRoomList = new List<RoomInfo>();

    private void Start()
    {
        PhotonNetwork.NetworkingClient.LoadBalancingPeer.DisconnectTimeout = 10000;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRoomList = roomList;
        Debug.Log($"Rooms disponibles: {cachedRoomList.Count}");
    }

    public void Join()
    {
        roomName = roomInput.text;

        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "DefaultRoom";
        }

        bool roomExists = cachedRoomList.Exists(room => room.Name == roomName);

        Debug.Log($"Sala '{roomName}' existe? {roomExists}");

        RoomOptions options = new RoomOptions { MaxPlayers = 4 };

        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene("LobbyManagerScreen", LoadSceneMode.Single);
    }

    public void Return()
    {
        Debug.Log("Volviendo menú...");
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("MainScreen", LoadSceneMode.Single);
    }
}
