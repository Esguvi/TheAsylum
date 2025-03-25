using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneScript : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("JUGAR");
    }

    public void Options()
    {
        Debug.Log("OPCIONES");
        SceneManager.LoadScene("OptionsScreen");
    }

    public void Credits()
    {
        Debug.Log("CREDITOS");
    }

    public void Exit()
    {
        Debug.Log("SALIR");

        Application.Quit();
    }
}
