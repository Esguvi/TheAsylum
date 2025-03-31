using UnityEngine;

public class AudioScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (transform.GetComponent<AudioSource>().volume != AudioConfScript.volume)
        {
            transform.GetComponent<AudioSource>().volume = AudioConfScript.volume;
        }
    }
}
