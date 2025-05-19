using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LlaveInteractableScript : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI objectiveText;

    public GameObject grabPoint;
    public float interactDistance = 3f;

    public Transform handPosition;
    public GameObject llaveVisible;

    public Invantory inventory;
    public CollectableObject llave;

    private bool isKeyEquipped = false;
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

            if ( tag == "Llave")
            {
                ObjectLocalizer hitLocalizer = hit.collider.GetComponent<ObjectLocalizer>();
                if (hitLocalizer != null)
                {
                    interactText.text = hitLocalizer.GetLocalizedName();
                    interactText.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
 

                        LlaveEquipada();
                    }

                }
            }
  
    }
    private void LlaveEquipada()
    {
       
        hit = GetRaycastHitFromGrabPoint();
        GameObject keyObject = hit.collider.GameObject();
        inventory.AddItemToInvanntory(llave);
        isKeyEquipped = true;

        llaveVisible.transform.SetParent(handPosition);
        llaveVisible.transform.localPosition = new Vector3(18.1f, -2.6f, 2f);
        llaveVisible.transform.localRotation = Quaternion.Euler(86.48f, 174.31f, 180f);
        llaveVisible.transform.localScale = new Vector3(20f, 20f, 20f);



        if (llave.TryGetComponent(out CapsuleCollider col))
            col.enabled = false;

        if (llave.TryGetComponent(out Rigidbody rb))
            rb.isKinematic = true;
        if (objectiveText != null)
            objectiveText.text = "Objetivo: Explora el entorno";
        if (isKeyEquipped)
        {
            Destroy(keyObject);
            Debug.Log("Llave del entorno destruida");
        }
        isKeyEquipped = true;
        HideInteractText();

    }





   

    private RaycastHit GetRaycastHitFromGrabPoint()
    {
        if (Physics.Raycast(grabPoint.transform.position, grabPoint.transform.forward, out RaycastHit hit, interactDistance))
        {
            return hit;
        }

        return default;
    }


    private void HideInteractText()
    {
        interactText.gameObject.SetActive(false);
    }
}