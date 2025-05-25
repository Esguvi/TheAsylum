using UnityEngine;
using UnityEngine.Video;

public class AudioScript : MonoBehaviour
{
    void Update()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        if (audioSource != null && audioSource.volume != AudioConfScript.volume)
        {
            audioSource.volume = AudioConfScript.volume;
        }
        else if (videoPlayer != null && videoPlayer.GetDirectAudioVolume(0) != AudioConfScript.volume)
        {
            videoPlayer.SetDirectAudioVolume(0, AudioConfScript.volume);
        }
    }
}
