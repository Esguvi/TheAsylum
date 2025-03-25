using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsBtnsScripts : MonoBehaviour
{
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
        SceneManager.LoadScene("MainScreen");
    }

}
