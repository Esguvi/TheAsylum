using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameOverScripts : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainScreen",LoadSceneMode.Single);
    }

    public void replay()
    {
        SceneManager.LoadScene("GameScreen", LoadSceneMode.Single);
    }
}
