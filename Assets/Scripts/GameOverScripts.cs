using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameOverScripts : MonoBehaviour
{    
    private PersistentVideo persistentVideo;
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        persistentVideo = FindObjectOfType<PersistentVideo>();
        if (persistentVideo != null)
        {
            persistentVideo.gameObject.SetActive(false);
        }

    }
    public void mainMenu()
    {
        if (persistentVideo != null)
        {
            persistentVideo.gameObject.SetActive(true);
        }
        SceneManager.LoadScene("MainScreen",LoadSceneMode.Single);
        
    }

    public void replay()
    {
        SceneManager.LoadScene("GameScreen", LoadSceneMode.Single);
    }
}
