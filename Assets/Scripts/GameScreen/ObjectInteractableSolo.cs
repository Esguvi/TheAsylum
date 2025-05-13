using System;
using TMPro;
using UnityEngine;

public class ObjectInteractableSolo : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    private ObjectLocalizer localizer;

    public GameObject grabPoint;
    public float interactDistance = 3f;

    public Transform handPosition;
    public GameObject flashLight;
    public Transform objectsParent;


    private bool isFlashlightEquipped = false;
    private bool isFlashlightOn = false;

    public static bool showDoorText = false;

    void Start()
    {
        interactText.gameObject.SetActive(false);
        localizer = GetComponent<ObjectLocalizer>();
        showDoorText = false;
    }

    void Update()
    {
        RaycastHit hit = GetRaycastHitFromGrabPoint();


        if (hit.collider != null && hit.collider.CompareTag("FlashLight"))
        {
            localizer = hit.collider.gameObject.GetComponent<ObjectLocalizer>();
            interactText.text = $"{localizer.GetLocalizedName()}";
            interactText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                EquipFlashLight();
            }
        }
        else if (!showDoorText)
        {
            interactText.gameObject.SetActive(false);
        }

        if (isFlashlightEquipped && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if (isFlashlightEquipped && Input.GetKeyDown(KeyCode.G))
        {
            DropFlashLight();
        }

    }

    private void EquipFlashLight()
    {
        if (handPosition != null)
        {
            transform.SetParent(handPosition);

            transform.localPosition = new Vector3(0.0667999983f, 0.0074f, 0.103100002f);
            transform.localRotation = Quaternion.Euler(356.111847f, 336.274353f, 69.830574f);
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            CapsuleCollider col = GetComponent<CapsuleCollider>();
            if (col != null) col.enabled = false;

            if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true; 
                rb.useGravity = false;
            }

            isFlashlightEquipped = true;
            interactText.gameObject.SetActive(false);  
        }
    }


    private void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        flashLight.SetActive(isFlashlightOn);
        Debug.Log("Linterna " + (isFlashlightOn ? "ENCENDIDA" : "APAGADA"));
    }

    private void DropFlashLight()
    {
        if (handPosition != null && objectsParent != null)
        {
            transform.SetParent(objectsParent);

            if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }

            CapsuleCollider col = GetComponent<CapsuleCollider>();
            if (col != null) col.enabled = true;

            if (isFlashlightOn)
            {
                isFlashlightOn = false;
            }

            isFlashlightEquipped = false;
        }
    }

    private RaycastHit GetRaycastHitFromGrabPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(grabPoint.transform.position, grabPoint.transform.forward, out hit, interactDistance))
        {
            return hit;
        }

        return default(RaycastHit);
    }
}