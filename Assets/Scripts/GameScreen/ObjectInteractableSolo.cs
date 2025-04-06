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

    private bool isFlashlightEquipped = false;
    private bool isFlashlightOn = false;

    public static bool showDoorText = false;

    void Start()
    {
        interactText.gameObject.SetActive(false);
        localizer = GetComponent<ObjectLocalizer>();
    }

    void Update()
    {
        RaycastHit hit = GetRaycastHitFromGrabPoint();

        if (hit.collider != null && hit.collider.CompareTag("FlashLight"))
        {
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
    }

    private void EquipFlashLight()
    {
        if (handPosition != null)
        {
            transform.SetParent(handPosition);
            transform.localPosition = new Vector3(18.1000004f, -2.5999999f, 2f);
            transform.localRotation = Quaternion.Euler(86.4812469f, 174.312363f, 180.000015f);
            transform.localScale = new Vector3(20, 20, 20);

            if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }

            isFlashlightEquipped = true;

            Debug.Log("Linterna equipada en la mano.");
        }
        else
        {
            Debug.LogWarning("HandPosition no asignado en el Inspector.");
        }

        interactText.gameObject.SetActive(false);
    }

    private void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        flashLight.SetActive(isFlashlightOn);
        Debug.Log("Linterna " + (isFlashlightOn ? "ENCENDIDA" : "APAGADA"));
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
