using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class RoomSelector : MonoBehaviour
{
    public static string roomName;
    public TextMeshProUGUI room; 

    public void Join()
    {
        roomName = room.text;
        SceneManager.LoadScene("MultiplayerScreen", LoadSceneMode.Single);
    }
}
