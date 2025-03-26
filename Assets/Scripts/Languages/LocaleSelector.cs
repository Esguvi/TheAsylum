using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System.Collections;

public class LocaleSelector : MonoBehaviour
{
    public RawImage imageBtn;
    public Texture esTexture; 
    public Texture enTexture; 

    private bool active = false;

    private void Start()
    {
        int localeID = PlayerPrefs.GetInt("LocaleKey", 0);
        StartCoroutine(SetLocale(localeID));
    }

    public void ToggleLanguage()
    {
        if (active)
            return;

        int newLocaleID = (PlayerPrefs.GetInt("LocaleKey", 0) == 0) ? 1 : 0;
        PlayerPrefs.SetInt("LocaleKey", newLocaleID);
        PlayerPrefs.Save();

        StartCoroutine(SetLocale(newLocaleID));
    }

    IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];

        imageBtn.texture = (localeID == 0) ? esTexture : enTexture;

        active = false;
    }
}
