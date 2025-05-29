using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class InteractionManager : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    private LocalizeStringEvent localizeStringEvent;

    private void Start()
    {
        localizeStringEvent = interactText.GetComponent<LocalizeStringEvent>();
    }

    public void SetInteractionText(string key)
    {
        // Cambiar la referencia del texto según la clave proporcionada
        localizeStringEvent.StringReference.SetReference("StringTable", key); // Asegúrate de que el "TableName" sea el nombre de tu archivo de localización
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto tiene el Layer 'Door'
        if (other.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            SetInteractionText("door"); // Usamos la clave de la puerta
        }

        // Puedes agregar más capas para otros objetos si lo necesitas
        else if (other.gameObject.layer == LayerMask.NameToLayer("Flashlight"))
        {
            SetInteractionText("flashlight"); // Usamos la clave de la linterna
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Door") || other.gameObject.layer == LayerMask.NameToLayer("Flashlight"))
        {
            interactText.text = ""; // Limpiar el texto cuando el jugador se aleja
        }
    }
}
