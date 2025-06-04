using UnityEngine;
using Firebase.Auth;
using Firebase.Firestore;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField userEmail;
    public TMP_InputField userPassword;
    public GameObject error;

    private FirebaseAuth auth;
    private FirebaseFirestore db;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        error.SetActive(false);
    }

    public void HandleLogin()
    {
        string email = userEmail.text.Trim();
        string password = userPassword.text;

        StartCoroutine(LoginUser(email, password));
    }

    IEnumerator LoginUser(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            error.SetActive(true);
        }
        else
        {
            FirebaseUser user = loginTask.Result.User;
            if (user != null)
            {
                StartCoroutine(LoadUsername(user));
            }
        }
    }

    IEnumerator LoadUsername(FirebaseUser user)
    {
        if (user == null)
        {
            Debug.LogError("Error: FirebaseUser es null.");
            yield break;
        }

        var userDoc = db.Collection("users").Document(user.UserId).GetSnapshotAsync();
        yield return new WaitUntil(() => userDoc.IsCompleted);

        if (userDoc.Exception != null)
        {
            Debug.LogError("Error obteniendo el username: " + userDoc.Exception);
            yield break;
        }

        DocumentSnapshot snapshot = userDoc.Result;
        string username = "Invitado";

        if (snapshot.Exists && snapshot.ContainsField("username"))
        {
            username = snapshot.GetValue<string>("username");
        }
        else
        {
            username = user.Email.Split('@')[0];
        }

        if (SessionManager.Instance != null)
        {
            SessionManager.Instance.SetUser(username);
        }

        SceneManager.LoadScene("MainScreen");
    }
}
