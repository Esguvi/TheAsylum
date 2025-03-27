using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ScreenScripts : MonoBehaviour
{
    public GameObject drop;
    public GameObject full;

    private bool active;

    private TMP_Dropdown dropdown;
    private Toggle fullScr;
    private void Start()
    {
        int localeID = PlayerPrefs.GetInt("LocaleKey", 0);
        StartCoroutine(SetLocale(localeID));
        dropdown = drop.GetComponent<TMP_Dropdown>();
        fullScr = full.GetComponent<Toggle>();
        dropdown.value = localeID;
        fullScr.isOn = Screen.fullScreen;
    }
    public void changeLenguage()
    {
        if (active) return;

        int newLocaleID = dropdown.value;
        PlayerPrefs.SetInt("LocaleKey", newLocaleID);
        PlayerPrefs.Save();

        StartCoroutine(SetLocale(newLocaleID));
    }

    IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        active = false;
    }

    public void fullScreen()
    {
        Screen.fullScreen = fullScr.isOn;
        Debug.Log(fullScr.isOn);
    }
}
