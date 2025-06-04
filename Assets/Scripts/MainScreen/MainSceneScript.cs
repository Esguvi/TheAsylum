using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainSceneScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameScreen", LoadSceneMode.Single);
    }

    public void MultiPlayer()
    {
        SceneManager.LoadScene("MultiplayerScreen", LoadSceneMode.Single);
    }

    [System.Obsolete]
    public void Options()
    {
        SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
        OptionsBtnsScripts.backScene = false;
        StartCoroutine(CleanupSceneConflicts());
    }

    public void Credits()
    {
    }

    public void Exit()
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
