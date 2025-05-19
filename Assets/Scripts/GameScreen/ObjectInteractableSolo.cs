using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectInteractableSolo : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI objectiveText;

    public GameObject grabPoint;
    public float interactDistance = 3f;

    public Transform handPosition;
    public GameObject flashLight;

    public Invantory inventory;
    public CollectableObject linterna;

    private bool isFlashlightEquipped = false;
    private bool isFlashlightOn = false;
    private bool isKeyEquipped = false;
    public static bool showDoorText = false;
    RaycastHit hit;
    void Start()
    {
        interactText.gameObject.SetActive(false);

        if (objectiveText != null)
        {
            objectiveText.text = "Objetivo: Consigue la linterna";
        }
    }

    void Update()
    {
         hit = GetRaycastHitFromGrabPoint();

        if (hit.collider != null)
        {
            string tag = hit.collider.tag;

            if (tag == "FlashLight" ||  tag == "Door")
            {
                ObjectLocalizer hitLocalizer = hit.collider.GetComponent<ObjectLocalizer>();
                if (hitLocalizer != null)
                {
                    interactText.text = hitLocalizer.GetLocalizedName();
                    interactText.gameObject.SetActive(true);
                }

                if (tag == "Door")
                {
                    showDoorText = true;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (tag == "FlashLight")
                    {
                      
                        EquipFlashLight(hit.collider.gameObject);
                        inventory.AddItemToInvanntory(linterna);
                        if (objectiveText != null)
                            objectiveText.text = "Objetivo: Explora el entorno";
                    }


                }
            }
            else if (!showDoorText)
            {
                HideInteractText();
            }
        }
        else
        {
            if (!showDoorText)
            {
                HideInteractText();
            }
        }

        if (isFlashlightEquipped && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }
    
  
    
    private void EquipFlashLight(GameObject flashlightObject)
    {
        
        flashlightObject.transform.SetParent(handPosition);
        flashlightObject.transform.localPosition = new Vector3(18.1f, -2.6f, 2f);
        flashlightObject.transform.localRotation = Quaternion.Euler(86.48f, 174.31f, 180f);
        flashlightObject.transform.localScale = new Vector3(20f, 20f, 20f);

        if (flashlightObject.TryGetComponent(out CapsuleCollider col))
            col.enabled = false;

        if (flashlightObject.TryGetComponent(out Rigidbody rb))
            rb.isKinematic = true;

        isFlashlightEquipped = true;
        HideInteractText();
    }
    


    private void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        flashLight.SetActive(isFlashlightOn);
        Debug.Log("Linterna " + (isFlashlightOn ? "ENCENDIDA" : "APAGADA"));
    }

    private RaycastHit GetRaycastHitFromGrabPoint()
    {
        if (Physics.Raycast(grabPoint.transform.position, grabPoint.transform.forward, out RaycastHit hit, interactDistance))
        {
            return hit;
        }

        return default;
    }

    public void ShowDoorText(string message)
    {
        interactText.text = message;
        interactText.gameObject.SetActive(true);
        showDoorText = true;
    }

    public void HideDoorText()
    {
        HideInteractText();
        showDoorText = false;
    }

    private void HideInteractText()
    {
        interactText.gameObject.SetActive(false);
    }
}
