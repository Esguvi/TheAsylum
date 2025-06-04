using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    public CollectableObject llaveAzul;
    public CollectableObject llaveRoja;
    public CollectableObject llaveVerde;
    public CollectableObject llaveFinal;
    public GameObject note;

    public Invantory inventory;

    private bool isFlashlightEquipped = false;
    private bool isFlashlightOn = false;
    private int isKeyEquipped = 0;

    private ObjectLocalizer localizer;
    private GameObject currentObject;
    private string currentTag;
    private List<GameObject> objectsInInventory;

    public static bool showDoorText = false;

    void Start()
    {
        interactText.gameObject.SetActive(false);
        showDoorText = false;
        objectsInInventory = new List<GameObject>();
    }

    void Update()
    {
        RaycastHit hit = GetRaycastHitFromGrabPoint();


        if (hit.collider != null && (hit.collider.CompareTag("FlashLight") || hit.collider.CompareTag("Llave") /*|| hit.collider.CompareTag("Note")*/))
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
                    objectsInInventory.Add(currentObject);

                }
                else if (currentTag == "Llave")
                {
                    EquipObject(currentObject, new Vector3(-0.0865f, 0.0377f, 0.0611f), Quaternion.Euler(351.4f, 271.15f, 265.2f), Vector3.one * 4f, handPositionR);
                    switch (hit.collider.name) {
                        case "Llave Azul" :
                            inventory.AddItemToInvanntory(llaveAzul);
                            break;
                        case "Llave Roja":
                            inventory.AddItemToInvanntory(llaveRoja);
                            break;
                        case "Llave Verde":
                            inventory.AddItemToInvanntory(llaveVerde);
                            break;
                        case "Llave Final":
                            inventory.AddItemToInvanntory(llaveFinal);
                            break;
                    };
                    
                    isKeyEquipped++;
                    objectsInInventory.Add(currentObject);
                }
                else if (currentTag == "Note")
                {
                    //NoteObject noteObj = currentObject.GetComponent<NoteObject>();
                    //if (noteObj != null)
                    //{
                    //    noteObj.PickUpNote();
                    //}
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

            //if (Input.GetKeyDown(KeyCode.G))
            //{
            //    DropObject(currentObject, linterna, handPositionL);
            //    isFlashlightEquipped = false;
            //    isFlashlightOn = false;
            //}
        }

        //if (isKeyEquipped > 0 && Input.GetKeyDown(KeyCode.G))
        //{
        //    DropObject(currentObject, llaveAzul, handPositionR);
        //    DropObject(currentObject, llaveRoja, handPositionR);
        //    isKeyEquipped --;
        //}


        if (Input.GetKeyDown(KeyCode.G))
        { 
            DropObject();
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

    private void DropObject()
    {
        //int index = inventory.BuscarObjetoPorNombre(collectable.name);
        //if (inventory.CurrentlySelectedItem != index)
        //{
        //    Debug.Log($"No puedes soltar {collectable.name} si no está seleccionado.");
        //    return;
        //}

        int index = inventory.CurrentlySelectedItem;
        GameObject obj;
        try
        {
            obj = objectsInInventory[index];
        } catch (ArgumentOutOfRangeException)
        {
            return;
        }

        obj.transform.SetParent(objectsParent);
        //obj.transform.position = handPosition.position + handPosition.forward * 0.5f;

        if (obj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            //rb.AddForce(Vector3.down*20f, ForceMode.Acceleration);
        }

        if (obj.TryGetComponent<CapsuleCollider>(out var col))
            col.enabled = true;

        inventory.RemoveItemFromInventory(index);
        objectsInInventory.Remove(obj);
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