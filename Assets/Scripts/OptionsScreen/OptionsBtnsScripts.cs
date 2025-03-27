using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OptionsBtnsScripts : MonoBehaviour
{

    public static bool backScene;

    public GameObject resumeBtn;
    public GameObject audioConf;
    public GameObject keyMoConf;
    public GameObject screenConf;
    public VideoPlayer videoPlayer;
    public VideoClip video;

    private void disableAll()
    {
        audioConf.SetActive(false);
        keyMoConf.SetActive(false);
        screenConf.SetActive(false);
}

    private void Start()
    {
        disableAll();
        audioConf.SetActive(true);
        if (backScene)
        {
            Debug.Log("ENTRO");
            videoPlayer.Stop();
            resumeBtn.SetActive(true);
            videoPlayer.clip = video;
            videoPlayer.Play();
        }
        else
        {
            resumeBtn.SetActive(false);
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
    }
    public void Resume()
    {
        Debug.Log("Reanudar");

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("OptionsScreen"));

        MovementScript.LockMouse();
    }

    public void ConfAudio()
    {
        Debug.Log("AUDIO");
        disableAll();
        audioConf.SetActive(true);
    }

    public void ConfKeys()
    {
        Debug.Log("TECLADO / RATÓN");
        disableAll();
        keyMoConf.SetActive(true);
    }
    public void ConfScreen()
    {
        Debug.Log("PANTALLA");
        disableAll();
        screenConf.SetActive(true);
    }

    public void Return()
    {
        Debug.Log("VOLVER");
        if (backScene)
        {
            SceneManager.LoadScene("MainScreen");
        }
        else
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("OptionsScreen"));
        }
    }

}
