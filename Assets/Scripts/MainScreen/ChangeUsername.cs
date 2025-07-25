using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using Photon.Pun;

public class ChangeUsername : MonoBehaviour
{
    public TMP_Text usernameText;
    public LocalizedString localizedUsername;

    void Start()
    {
        if (SessionManager.Instance != null)
        {
            string username = SessionManager.Instance.username;
            if (string.IsNullOrEmpty(username))
            {
                username = "Invitado";
            }

            localizedUsername.Arguments = new object[] { username };
            localizedUsername.StringChanged += UpdateUsernameText;
            localizedUsername.RefreshString();
        }
    }

    void UpdateUsernameText(string value)
    {
        usernameText.text = value;
        PhotonNetwork.NickName = value;
    }
}
