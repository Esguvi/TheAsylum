using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioConfScript : MonoBehaviour
{
    public static float volume = 0.5f;

    public Slider slider;
    public TMP_Text txtValue;

    private void Start()
    {
        slider.value = volume*100;
    }

    public void changeVolum()
    {
        volume = slider.value/100;
        txtValue.text = Math.Round(slider.value, 3).ToString();
    }
}
