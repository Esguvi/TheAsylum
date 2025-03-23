using UnityEngine;

public class LoginManager : MonoBehaviour
{
    public void openWebPage()
    {
        Application.OpenURL("https://theasylum.vercel.app/account.html");
    }
}
