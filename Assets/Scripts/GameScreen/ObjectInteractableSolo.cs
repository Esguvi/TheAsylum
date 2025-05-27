using System;
using TMPro;
using UnityEngine;

public class ObjectInteractableSolo : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    private ObjectLocalizer localizer;

    public GameObject grabPoint;
    public float interactDistance;

    public Transform handPosition;
    public GameObject flashLight;
    public Transform objectsParent;

    public Invantory inventory;
    public CollectableObject linterna;

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
                inventory.AddItemToInvanntory(linterna);
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

            transform.localPosition = new Vector3(-0.0627999976f, 0.0763999969f, 0.132300004f);
            transform.localRotation = Quaternion.Euler(11.2050133f, 215.799973f, 86.432991f);
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
        if (flashLight.activeSelf)
        {
            flashLight.SetActive(false);
            Debug.Log("Linterna apagada");
        }
        else
        {
            flashLight.SetActive(true);
            Debug.Log("Linterna encendida");
        }
    }

    private void DropFlashLight()
    {
        int index = inventory.BuscarObjetoPorNombre("Linterna");
        if (inventory.CurrentlySelectedItem != index)
        {
            Debug.Log("No puedes soltar la linterna si no está seleccionada.");
            return;
        }
        if (handPosition != null && objectsParent != null)
        {
            transform.SetParent(objectsParent);
            transform.position = handPosition.transform.position + handPosition.transform.forward * 0.5f;

            if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                // FALTA A�ADIR MAS FUERZA DE CAIDA AL SOLTAR EL OBJETO
            }

            CapsuleCollider col = GetComponent<CapsuleCollider>();
            if (col != null) col.enabled = true;

            if (isFlashlightOn)
            {
                isFlashlightOn = false;
            }

            inventory.RemoveItemFromInventory(linterna);
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