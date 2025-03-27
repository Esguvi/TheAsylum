using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyMoConfScript : MonoBehaviour
{
    public static float sensibility = 2;

    public Slider slider;
    public TMP_Text txtValue;

    private void Start()
    {
         slider.value = sensibility;
    }

    public void changeSensibility()
    {
        sensibility = slider.value;
        txtValue.text = Math.Round(slider.value,3).ToString();
    }
}
