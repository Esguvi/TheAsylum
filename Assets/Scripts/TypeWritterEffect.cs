using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text textMesh;
    public float fadeDuration = 2f;

    void Start()
    {
        StartCoroutine(FadeText());
    }

    IEnumerator FadeText()
    {
        textMesh.alpha = 0; 
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            textMesh.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
    }
}
