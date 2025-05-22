using TMPro;
using UnityEngine;

public class LlaveInteractableScript : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    private ObjectLocalizer localizer;
    public GameObject grabPoint;
    public float interactDistance = 3f;

    public Transform handPosition;
    public GameObject llaveVisible;
    public Transform objectsParent;
    public Invantory inventory;
    public CollectableObject llave;
    public static bool showDoorText = false;
    private bool isKeyEquipped = false;
    RaycastHit hit;

    void Start()
    {
        interactText.gameObject.SetActive(false);
        localizer = GetComponent<ObjectLocalizer>();
        showDoorText = false;
    }

    void Update()
    {
        hit = GetRaycastHitFromGrabPoint();



        if (hit.collider != null && hit.collider.CompareTag("Llave"))
        {
            localizer = hit.collider.gameObject.GetComponent<ObjectLocalizer>();
            interactText.text = $"{localizer.GetLocalizedName()}";
            interactText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {

                inventory.AddItemToInvanntory(llave);
                LlaveEquipada();
            }
            if (isKeyEquipped && Input.GetKeyDown(KeyCode.G))
            {
                DropKey();
            }

        }
        else if (!showDoorText)
        {
            interactText.gameObject.SetActive(false);
        }

    }
    private void LlaveEquipada()
    {

        if (handPosition != null)
        {
            transform.SetParent(handPosition);

            transform.localPosition = new Vector3(-0.0627999976f, 0.0763999969f, 0.132300004f);
            transform.localRotation = Quaternion.Euler(11.2050133f, 215.799973f, 86.432991f);
            transform.localScale = new Vector3(100f, 1003f, 100f);

            CapsuleCollider col = GetComponent<CapsuleCollider>();
            if (col != null) col.enabled = false;

            if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            isKeyEquipped = true;
            interactText.gameObject.SetActive(false);
        }

    }


    private void DropKey()
    {
        int index = inventory.BuscarObjetoPorNombre("Llave");
        if (inventory.CurrentlySelectedItem != index)
        {
            Debug.Log("No puedes soltar la llave si no está seleccionada.");
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
                
                // FALTA AÑADIR MAS FUERZA DE CAIDA AL SOLTAR EL OBJETO
            }

            CapsuleCollider col = GetComponent<CapsuleCollider>();
            if (col != null) col.enabled = true;

            inventory.RemoveItemFromInventory(llave);
            isKeyEquipped = false;
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