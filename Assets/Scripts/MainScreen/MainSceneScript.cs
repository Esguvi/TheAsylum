using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainSceneScript : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("JUGAR");
        SceneManager.LoadScene("GameScreen", LoadSceneMode.Single);
    }

    public void MultiPlayer()
    {
        Debug.Log("MULTIJUGADOR");
        SceneManager.LoadScene("MultiplayerScreen", LoadSceneMode.Single);
    }

    [System.Obsolete]
    public void Options()
    {
        Debug.Log("OPCIONES");
        SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
        OptionsBtnsScripts.backScene = false;
        StartCoroutine(CleanupSceneConflicts());
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

    [System.Obsolete]
    private IEnumerator CleanupSceneConflicts()
    {
        yield return null;

        AudioListener[] listeners = FindObjectsOfType<AudioListener>();
        bool foundAudio = false;
        foreach (var listener in listeners)
        {
            if (!foundAudio)
            {
                foundAudio = true;
            }
            else
            {
                listener.enabled = false;
            }
        }

        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        bool foundEvent = false;
        foreach (var ev in eventSystems)
        {
            if (!foundEvent)
            {
                foundEvent = true;
            }
            else
            {
                ev.gameObject.SetActive(false);
            }
        }
    }
}
