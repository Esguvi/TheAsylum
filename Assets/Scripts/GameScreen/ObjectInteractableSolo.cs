using System;
using TMPro;
using UnityEngine;

public class ObjectInteractableSolo : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public GameObject grabPoint;
    private float interactDistance = 100;

    public Transform handPositionR;
    public Transform handPositionL;
    public Transform objectsParent;

    public GameObject flashLight;
    public CollectableObject linterna;
    public CollectableObject llave;
    public GameObject note;

    public Invantory inventory;

    private bool isFlashlightEquipped = false;
    private bool isFlashlightOn = false;
    private bool isKeyEquipped = false;

    private ObjectLocalizer localizer;
    private GameObject currentObject;
    private string currentTag;

    public static bool showDoorText = false;

    void Start()
    {
        interactText.gameObject.SetActive(false);
        showDoorText = false;
    }

    void Update()
    {
        RaycastHit hit = GetRaycastHitFromGrabPoint();

        if (hit.collider != null && (hit.collider.CompareTag("FlashLight") || hit.collider.CompareTag("Llave") || hit.collider.CompareTag("Note")))
        {
            currentObject = hit.collider.gameObject;
            currentTag = hit.collider.tag;
            localizer = currentObject.GetComponent<ObjectLocalizer>();
            interactText.text = $"{localizer.GetLocalizedName()}";
            interactText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentTag == "FlashLight")
                {
                    EquipObject(currentObject, new Vector3(-0.0628f, 0.0764f, 0.1323f), Quaternion.Euler(11.2f, 215.8f, 86.4f), Vector3.one * 0.3f, handPositionL);
                    inventory.AddItemToInvanntory(linterna);
                    isFlashlightEquipped = true;
                }
                else if (currentTag == "Llave")
                {
                    EquipObject(currentObject, new Vector3(-0.0865f, 0.0377f, 0.0611f), Quaternion.Euler(351.4f, 271.15f, 265.2f), Vector3.one * 4f, handPositionR);
                    inventory.AddItemToInvanntory(llave);
                    isKeyEquipped = true;
                }
                else if (currentTag == "Note")
                {
                    NoteObject noteObj = currentObject.GetComponent<NoteObject>();
                    if (noteObj != null)
                    {
                        noteObj.PickUpNote();
                    }
                }
            }
        }
        else if (!showDoorText)
        {
            interactText.gameObject.SetActive(false);
        }

        if (isFlashlightEquipped)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleFlashlight();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                DropObject(currentObject, linterna, handPositionL);
                isFlashlightEquipped = false;
                isFlashlightOn = false;
            }
        }

        if (isKeyEquipped && Input.GetKeyDown(KeyCode.G))
        {
            DropObject(currentObject, llave, handPositionR);
            isKeyEquipped = false;
        }
    }

    private void EquipObject(GameObject obj, Vector3 localPos, Quaternion localRot, Vector3 localScale, Transform handPosition)
    {
        obj.transform.SetParent(handPosition);
        obj.transform.localPosition = localPos;
        obj.transform.localRotation = localRot;
        obj.transform.localScale = localScale;

        if (obj.TryGetComponent<CapsuleCollider>(out var col))
            col.enabled = false;

        if (obj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        interactText.gameObject.SetActive(false);
    }

    private void DropObject(GameObject obj, CollectableObject collectable, Transform handPosition)
    {
        int index = inventory.BuscarObjetoPorNombre(collectable.nombreObjeto);
        if (inventory.CurrentlySelectedItem != index)
        {
            Debug.Log($"No puedes soltar {collectable.nombreObjeto} si no está seleccionado.");
            return;
        }

        obj.transform.SetParent(objectsParent);
        obj.transform.position = handPosition.position + handPosition.forward * 0.5f;

        if (obj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(handPosition.forward * 2f, ForceMode.Impulse);
        }

        if (obj.TryGetComponent<CapsuleCollider>(out var col))
            col.enabled = true;

        inventory.RemoveItemFromInventory(collectable);
    }

    private void ToggleFlashlight()
    {
        if (flashLight == null)
        {
            Debug.LogWarning("FlashLight no está asignado.");
            return;
        }
        else
        {
            flashLight.SetActive(!flashLight.activeSelf);
            isFlashlightOn = flashLight.activeSelf;
            Debug.Log(isFlashlightOn ? "Linterna encendida" : "Linterna apagada");
        }
    }

    private RaycastHit GetRaycastHitFromGrabPoint()
    {
        if (Physics.Raycast(grabPoint.transform.position, grabPoint.transform.forward, out var hit, interactDistance))
        {
            return hit;
        }

        return default;
    }
}
