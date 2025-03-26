using UnityEngine;
using UnityEngine.Video;

public class PersistentVideo : MonoBehaviour
{
    private static PersistentVideo instance;
    private VideoPlayer videoPlayer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            videoPlayer = GetComponent<VideoPlayer>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
