using UnityEngine;
using UnityEngine.Localization;

public class ObjectLocalizer : MonoBehaviour
{
    public LocalizedString localizedObjectName;

    public string GetLocalizedName()
    {
        string name = localizedObjectName.GetLocalizedString();
        return name;
    }
}