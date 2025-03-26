using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OptionsBtnsScripts : MonoBehaviour
{

    public static bool backScene;

    public GameObject resumeBtn;
    public VideoPlayer videoPlayer;
    public VideoClip video;

    private void Start()
    {
        if (backScene)
        {
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
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ConfAudio()
    {
        Debug.Log("AUDIO");
    }

    public void Resume()
    {
        Debug.Log("Reanudar");

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("OptionsScreen"));

        MovementScript.LockMouse();
    }

    public void ConfKeys()
    {
        Debug.Log("TECLADO / RATÓN");
    }
    public void ConfScreen()
    {
        Debug.Log("PANTALLA");
    }

    public void Return()
    {
        Debug.Log("VOLVER");
        if(backScene)
        {
            SceneManager.LoadScene("MainScreen");
        }
        else
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("OptionsScreen"));
        }
    }

}
