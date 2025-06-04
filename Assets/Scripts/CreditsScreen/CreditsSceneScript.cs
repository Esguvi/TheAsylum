using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CreditsSceneScript : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    public void Return()
    {
        SceneManager.LoadScene("MainScreen", LoadSceneMode.Single);
    }

}
