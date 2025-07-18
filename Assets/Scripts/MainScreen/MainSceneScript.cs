using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainSceneScript : MonoBehaviour
{
    public void StartGame()
    {
        CargaNivel.SceneLoader("GameScreen");
    }

    public void MultiPlayer()
    {
        CargaNivel.SceneLoader("RoomManagerScreen");
        StartCoroutine(CleanupSceneConflicts());
    }

    public void Options()
    {
        SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
        OptionsBtnsScripts.backScene = false;
        StartCoroutine(CleanupSceneConflicts());
    }

    public void Credits()
    {
        PersistentVideo persistentVideo = FindObjectOfType<PersistentVideo>();
        if (persistentVideo != null)
        {
            Destroy(persistentVideo.gameObject);
        }

        SceneManager.LoadScene("CreditsScreen", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }

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
