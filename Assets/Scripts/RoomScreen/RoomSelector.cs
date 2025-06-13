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
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        cachedRoomList = roomList;
    }

    public void Join()
    {
        roomName = roomInput.text;

        if (string.IsNullOrEmpty(roomName))
        {
            roomName = "DefaultRoom";
        }

        bool roomExists = cachedRoomList.Exists(room => room.Name == roomName);

        RoomOptions options = new RoomOptions { MaxPlayers = 4 };

        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("LobbyManagerScreen", LoadSceneMode.Single);
    }

    public void Return()
    {
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("MainScreen", LoadSceneMode.Single);
    }
}
