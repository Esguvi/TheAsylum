using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsBtnsScripts : MonoBehaviour
{
    public static string backScene;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

    }
    public void ConfAudio()
    {
        Debug.Log("AUDIO");
    }

    public void ConfKeys()
    {
        Debug.Log("KEYS");
    }
    public void ConfGrafics()
    {
        Debug.Log("GRAFICOS");
    }

    public void Exit()
    {
        Debug.Log("SALIR");
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("OptionsScreen"));
    }

}
