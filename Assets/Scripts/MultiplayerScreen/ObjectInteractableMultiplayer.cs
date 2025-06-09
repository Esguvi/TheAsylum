using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectInteractableMultiplayer : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI interactText;
    public GameObject grabPoint;
    private float interactDistance = 100;

    public Transform handPositionR;
    public Transform handPositionL;
    public Transform objectsParent;

    public GameObject flashLight;
    public Vector3 flashLightPosition;
    public CollectableObject linterna;
    public CollectableObject llaveAzul;
    public CollectableObject llaveRoja;
    public CollectableObject llaveVerde;
    public CollectableObject llaveFinal;
    public Vector3 keyPosition;
    public GameObject note;

    public Invantory inventory;

    private bool isFlashlightEquipped = false;
    private bool isFlashlightOn = false;
    private int isKeyEquipped = 0;

    private ObjectLocalizer localizer;
    private GameObject currentObject;
    private string currentTag;
    public List<GameObject> objectsInInventory;

    public static bool showDoorText = false;
    public static string externalInteractText = "";


    [Obsolete]
    void Start()
    {
        interactText.gameObject.SetActive(false);
        showDoorText = false;
        objectsInInventory = new List<GameObject>();

        StartCoroutine(WaitForObjectsToLoad());
    }


    void Update()
    {

        objectsParent = GameObject.Find("Objects").transform;
        flashLight = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name.Contains("FlashLight"));
        linterna = objectsParent.GetComponentsInChildren<CollectableObject>(true).FirstOrDefault(co => co.name.Contains("Flashlight"));
        llaveAzul = objectsParent.GetComponentsInChildren<CollectableObject>(true).FirstOrDefault(co => co.name.Contains("Llave Azul"));
        llaveRoja = objectsParent.GetComponentsInChildren<CollectableObject>(true).FirstOrDefault(co => co.name.Contains("Llave Roja"));
        llaveVerde = objectsParent.GetComponentsInChildren<CollectableObject>(true).FirstOrDefault(co => co.name.Contains("Llave Verde"));
        llaveFinal = objectsParent.GetComponentsInChildren<CollectableObject>(true).FirstOrDefault(co => co.name.Contains("Llave Final"));
        note = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name.Equals("Note"));


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
                    if (!photonView.IsMine)
                    {
                        return;
                    }
                    photonView.RPC("EquipObject", RpcTarget.All, currentObject.GetPhotonView().ViewID, new Vector3(-0.0628f, 0.0764f, 0.1323f), Quaternion.Euler(11.2f, 215.8f, 86.4f), Vector3.one * 0.3f, "Left");
                    //EquipObject(currentObject, new Vector3(-0.0628f, 0.0764f, 0.1323f), Quaternion.Euler(11.2f, 215.8f, 86.4f), Vector3.one * 0.3f, handPositionL);
                    Debug.Log(linterna);
                    inventory.AddItemToInvanntory(linterna);
                    isFlashlightEquipped = true;
                    objectsInInventory.Add(currentObject);
                }
                else if (currentTag == "Llave")
                {
                    if (!photonView.IsMine)
                    {
                        return;
                    }
                    photonView.RPC("EquipObject", RpcTarget.All, currentObject.GetPhotonView().ViewID, new Vector3(-0.0865f, 0.0377f, 0.0611f), Quaternion.Euler(351.4f, 271.15f, 265.2f), Vector3.one * 4f, "Right");
                    //EquipObject(currentObject, new Vector3(-0.0865f, 0.0377f, 0.0611f), Quaternion.Euler(351.4f, 271.15f, 265.2f), Vector3.one * 4f, handPositionR);
                    switch (hit.collider.name)
                    {
                        case "Llave Azul":
                            inventory.AddItemToInvanntory(llaveAzul);
                            break;
                        case "Llave Roja":
                            inventory.AddItemToInvanntory(llaveRoja);
                            break;
                    };

                    isKeyEquipped++;
                    objectsInInventory.Add(currentObject);
                }
                else if (currentTag == "Note")
                {
                    if (!photonView.IsMine)
                    {
                        return;
                    }
                    NoteObject noteObj = currentObject.GetComponent<NoteObject>();
                    if (noteObj != null)
                    {
                        noteObj.PickUpNote(currentObject.transform.parent.gameObject);
                    }
                }
            }
        }
        else
        {
            if (showDoorText && !string.IsNullOrEmpty(externalInteractText))
            {
                interactText.text = externalInteractText;
                interactText.gameObject.SetActive(true);
            }
            else
            {
                interactText.gameObject.SetActive(false);
            }
        }

        if (isFlashlightEquipped && Input.GetKeyDown(KeyCode.F))
        {
            photonView.RPC("ToggleFlashlight", RpcTarget.AllBuffered, flashLight.GetPhotonView().ViewID);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            int index = inventory.CurrentlySelectedItem;

            if (index >= 0 && index < objectsInInventory.Count)
            {
                if (!photonView.IsMine)
                {
                    return;
                }
                int viewID = objectsInInventory[index].GetComponent<PhotonView>().ViewID;
                photonView.RPC("DropObjectByID", RpcTarget.All, viewID);
                Debug.Log("Objeto soltado correctamente");
            }
        }
    }

    [PunRPC]
    private void EquipObject(int objectViewID, Vector3 localPos, Quaternion localRot, Vector3 localScale, string hand)
    {
        GameObject obj = PhotonView.Find(objectViewID).gameObject;
        Transform handPosition = hand == "Left" ? handPositionL : handPositionR;

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

    [PunRPC]
    private void DropObjectByID(int viewID)
    {
        int index = inventory.CurrentlySelectedItem;
        GameObject obj = PhotonView.Find(viewID).gameObject;

        obj.transform.SetParent(objectsParent);
        obj.transform.position = grabPoint.transform.position + grabPoint.transform.forward * 0.5f;
        obj.transform.rotation = Quaternion.identity;

        if (obj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (obj.TryGetComponent<Collider>(out var col))
            col.enabled = true;

        inventory.RemoveItemFromInventory(index);
        objectsInInventory.Remove(obj);

        if (obj.name.Contains("Flashlight"))
        {
            linterna = obj.GetComponent<CollectableObject>();
        }
        else if (obj.name.Contains("Llave Azul"))
        {
            llaveAzul = obj.GetComponent<CollectableObject>();
        }
        else if (obj.name.Contains("Llave Roja"))
        {
            llaveRoja = obj.GetComponent<CollectableObject>();
        }
    }

    [PunRPC]
    private void ToggleFlashlight(int flashlightViewID)
    {
        GameObject obj = PhotonView.Find(flashlightViewID).gameObject;
        obj.SetActive(!obj.activeSelf);

        if (photonView.IsMine)
        {
            isFlashlightOn = obj.activeSelf;
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


    [Obsolete]
    private IEnumerator WaitForObjectsToLoad()
    {
        yield return new WaitForSeconds(1f);

        objectsParent = GameObject.Find("Objects").transform;

        flashLight = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name.Contains("FlashLight"));
        linterna = objectsParent.GetComponentsInChildren<CollectableObject>(true).FirstOrDefault(co => co.name.Contains("Flashlight"));
        llaveAzul = objectsParent.GetComponentsInChildren<CollectableObject>(true).FirstOrDefault(co => co.name.Contains("Llave Azul"));
        llaveRoja = objectsParent.GetComponentsInChildren<CollectableObject>(true).FirstOrDefault(co => co.name.Contains("Llave Roja"));
        llaveVerde = objectsParent.GetComponentsInChildren<CollectableObject>(true).FirstOrDefault(co => co.name.Contains("Llave Verde"));
        llaveFinal = objectsParent.GetComponentsInChildren<CollectableObject>(true).FirstOrDefault(co => co.name.Contains("Llave Final"));
        note = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(go => go.name.Equals("Note"));

        if (flashLight == null || linterna == null)
        {
            Debug.LogWarning("No se pudo encontrar la linterna u otros objetos importantes.");
        }
    }
}