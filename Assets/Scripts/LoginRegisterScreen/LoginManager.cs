using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField userEmail;
    public TMP_InputField userPassword;
    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void HandleLogin()
    {
        string email = userEmail.text;
        string password = userPassword.text;

        StartCoroutine(LoginUser(email, password));
    }

    IEnumerator LoginUser(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError("Login failed: " + loginTask.Exception);
        }
        else
        {
            FirebaseUser user = loginTask.Result.User;
            SceneManager.LoadScene("MainScreen");
        }
    }
}
