using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public RawImage imageBtn;
    public AudioSource audio;
    public Texture yesAudioTexture;
    public Texture noAudioTexture;

    private bool active = true;

    public void ToggleAudio()
    {
        active = !active;
        audio.volume = active ? 0.7f : 0;
        imageBtn.texture = active ? yesAudioTexture : noAudioTexture;
    }
}
