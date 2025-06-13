using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene("MainScreen", LoadSceneMode.Single);
    }

    public void replay()
    {
        if (persistentVideo != null)
        {
            persistentVideo.gameObject.SetActive(true);
        }
        SceneManager.LoadScene("GameScreen", LoadSceneMode.Single);
    }
}
