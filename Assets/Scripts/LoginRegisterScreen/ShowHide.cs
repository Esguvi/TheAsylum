using UnityEngine;

public class ShowHide : MonoBehaviour
{
    public GameObject gameObjectLogin;
    public GameObject gameObjectLoginBtn;
    public GameObject gameObjectRegisterBtn;

    public void showLoginForm()
    {
        gameObjectLogin.SetActive(true);
        gameObjectLoginBtn.SetActive(false);
        gameObjectRegisterBtn.SetActive(false);
    }

    public void returnForms()
    {
        gameObjectLogin.SetActive(false);
        gameObjectLoginBtn.SetActive(true);
        gameObjectRegisterBtn.SetActive(true);  
    }
}
