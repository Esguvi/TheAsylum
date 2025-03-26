using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsBtnsScripts : MonoBehaviour
{
    public static bool backScene;

    public GameObject resumeBtn;
    public TMP_Text homeBtn;

    private void Start()
    {
        if (backScene) 
        { 
            resumeBtn.SetActive(true);
            homeBtn.text = "Menu Principal";
        }
        else
        {
            resumeBtn.SetActive(false);
            homeBtn.text = "Volver";
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

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
