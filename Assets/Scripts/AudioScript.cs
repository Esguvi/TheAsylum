using UnityEngine;
using UnityEngine.Video;

public class AudioScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (transform.GetComponent<AudioSource>() != null && transform.GetComponent<AudioSource>().volume != AudioConfScript.volume)
        {
            transform.GetComponent<AudioSource>().volume = AudioConfScript.volume;
        }
        else if (transform.GetComponent<VideoPlayer>().GetDirectAudioVolume(0) != AudioConfScript.volume && transform.GetComponent<VideoPlayer>() != null)
        {
            transform.GetComponent<VideoPlayer>().SetDirectAudioVolume(0,AudioConfScript.volume);
        }
    }
}
