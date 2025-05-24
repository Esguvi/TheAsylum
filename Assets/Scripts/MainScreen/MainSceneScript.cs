using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneScript : MonoBehaviour
{
    public void StartGame()
    {
        CargaNivel.SceneLoader("GameScreen");
    }

    public void MultiPlayer()
    {
        SceneManager.LoadScene("RoomManager", LoadSceneMode.Single);
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
