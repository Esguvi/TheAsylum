using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class ObjectInteractableMultiplayer : MonoBehaviourPun
{
    public TextMeshProUGUI interactText;
    public GameObject grabPoint;
    public float interactDistance;
    public Transform handPosition;

    private ObjectPickup equippedObject;

    public LocalizedString flashlightText;

    void Start()
    {
        if (!photonView.IsMine)
        {
            interactText.gameObject.SetActive(false);
            enabled = false;
            return;
        }

        interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleInteractionRaycast();
        HandleObjectToggle();
    }

    private void HandleInteractionRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(grabPoint.transform.position, grabPoint.transform.forward, out hit, interactDistance))
        {
            if (hit.collider.CompareTag("FlashLight"))
            {
                flashlightText.TableReference = "StringTable";  
                flashlightText.TableEntryReference = "flashlight"; 

                interactText.text = flashlightText.GetLocalizedString();

                interactText.gameObject.SetActive(true);

                ObjectPickup objectPickup = hit.collider.GetComponent<ObjectPickup>();
                if (objectPickup != null && !objectPickup.IsEquipped)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        objectPickup.Equip(handPosition);
                        equippedObject = objectPickup;
                        interactText.gameObject.SetActive(false);
                    }

                    return;
                }
            }
        }

        interactText.gameObject.SetActive(false);
    }

    private void HandleObjectToggle()
    {
        if (equippedObject != null && Input.GetKeyDown(KeyCode.F))
        {
            equippedObject.Toggle();
        }
    }
}
