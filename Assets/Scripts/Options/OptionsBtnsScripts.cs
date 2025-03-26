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
        SceneManager.LoadScene("MainScreen");
    }

}
