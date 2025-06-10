using Photon.Pun;
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
    public AudioSource audioo;

    private PersistentVideo persistentVideo;

    [System.Obsolete]
    private void Start()
    {
        disableAll();
        audioConf.SetActive(true);

        if(SceneManager.sceneCount == 1)
        {
            Return();
        }

        if (backScene)
        {
            persistentVideo = FindObjectOfType<PersistentVideo>();
            if (persistentVideo != null)
            {
                persistentVideo.gameObject.SetActive(false);
            }
            videoPlayer.Stop();
            resumeBtn.SetActive(true);
            videoPlayer.clip = video;
            videoPlayer.Play();
        }
        else
        {
            resumeBtn.SetActive(false);
            audioo.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void disableAll()
    {
        audioConf.SetActive(false);
        keyMoConf.SetActive(false);
        screenConf.SetActive(false);
    }

    public void Resume()
    {
        MovementScript.cameraPlayerr.GetComponent<AudioListener>().enabled = true;
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("OptionsScreen"));
        MovementScript.LockMouse();
    }

    public void ConfAudio()
    {
        disableAll();
        audioConf.SetActive(true);
    }

    public void ConfKeys()
    {
        disableAll();
        keyMoConf.SetActive(true);
    }

    public void ConfScreen()
    {
        disableAll();
        screenConf.SetActive(true);
    }

    [System.Obsolete]
    public void Return()
    {
        if (backScene)
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("MainScreen");
            if (persistentVideo != null)
            {
                persistentVideo.gameObject.SetActive(true);
                audioo.enabled = true;
            }
        }
        else
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("OptionsScreen"));
        }
    }

}
