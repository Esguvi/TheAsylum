using UnityEngine;
using Photon.Pun;

public class PlayerInteraction : MonoBehaviourPun
{
    private InteractableObject currentInteractable = null;

    void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.E)) // Ejemplo tecla de interacción
        {
            if (currentInteractable != null)
            {
                currentInteractable.OnPickedUp();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) // Ejemplo soltar objeto
        {
            if (currentInteractable != null)
            {
                Vector3 dropPos = transform.position + transform.forward * 2f;
                currentInteractable.OnDropped(dropPos);
                currentInteractable = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            InteractableObject interactable = other.GetComponent<InteractableObject>();
            if (interactable != null && !interactable.photonView.IsMine)
            {
                currentInteractable = interactable;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            InteractableObject interactable = other.GetComponent<InteractableObject>();
            if (interactable == currentInteractable)
            {
                currentInteractable = null;
            }
        }
    }
}
