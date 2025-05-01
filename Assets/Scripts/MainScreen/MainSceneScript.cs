using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScreen", LoadSceneMode.Single);
    }

    public void MultiPlayer()
    {
        SceneManager.LoadScene("MultiplayerScreen", LoadSceneMode.Single);
    }

    public void Options()
    {
        SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
        OptionsBtnsScripts.backScene = false;
    }

    public void Credits()
    {
        Debug.Log("CREDITOS");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
