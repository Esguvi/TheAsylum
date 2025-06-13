using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargaNivel : MonoBehaviour
{
    public static string nextScene;

    private void Start()
    {
        StartCoroutine(StartLoading(nextScene));
    }

    IEnumerator StartLoading(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public static void SceneLoader (string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene ("LoadingScene");

    }
}
