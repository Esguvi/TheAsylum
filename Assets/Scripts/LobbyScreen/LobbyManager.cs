using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI player1;
    public TextMeshProUGUI player2;
    public TextMeshProUGUI player3;
    public TextMeshProUGUI player4;
    public TextMeshProUGUI roomNameText;
    public Button playButton;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playButton.gameObject.SetActive(true);
            playButton.interactable = true;
        }
        UpdateRoomData();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateRoomData();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateRoomData();
    }

    private void UpdateRoomData()
    {
        if (roomNameText != null)
            roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        player1.text = players.Length > 0 ? players[0].NickName : "TheAsylum...";
        player2.text = players.Length > 1 ? players[1].NickName : "TheAsylum...";
        player3.text = players.Length > 2 ? players[2].NickName : "TheAsylum...";
        player4.text = players.Length > 3 ? players[3].NickName : "TheAsylum...";
    }

    public void Play()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        playButton.interactable = false;
        PhotonNetwork.LoadLevel("MultiplayerScreen");
    }

    public void Return()
    {
        PhotonNetwork.LeaveRoom();
        CargaNivel.SceneLoader("MainScreen");
    }
}
