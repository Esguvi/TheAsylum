using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ObjectInteractableMultiplayerrr : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI interactText;
    private ObjectLocalizer localizer;

    private GameObject grabPoint;
    private Transform handPosition;
    private GameObject lightPart;

    private bool isFlashlightEquipped = false;
    private bool isFlashlightOn = false;

    private float interactDistance = 3f;
    private bool playerInitialized = false;

    void Start()
    {
        localizer = GetComponent<ObjectLocalizer>();
        TryFindPlayerComponents();
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (!playerInitialized)
        {
            TryFindPlayerComponents();
            return;
        }

        RaycastHit hit = GetRaycastHitFromGrabPoint();

        if (hit.collider != null && hit.collider.CompareTag("FlashLight"))
        {
            ObjectLocalizer hitLocalizer = hit.collider.GetComponent<ObjectLocalizer>();
            if (hitLocalizer != null)
                interactText.text = $"{hitLocalizer.GetLocalizedName()}";

            interactText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                EquipFlashLight(hit.collider.gameObject);
                Debug.Log("Linterna equipada.");
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }

        if (isFlashlightEquipped && Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    private void TryFindPlayerComponents()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null) return;

        interactText = player.transform.Find("Canvas/InteractionText")?.GetComponent<TextMeshProUGUI>();
        handPosition = player.transform.Find("Main Camera/HandPosition");
        grabPoint = player.transform.Find("Main Camera/GrabPoint")?.gameObject;

        if (interactText == null || handPosition == null || grabPoint == null)
        {
            Debug.LogWarning("Faltan referencias de player.");
            return;
        }

        interactText.gameObject.SetActive(false);
        playerInitialized = true;
    }

    private void EquipFlashLight(GameObject flashlight)
    {
        flashlight.transform.SetParent(handPosition);
        flashlight.transform.localPosition = new Vector3(18.1f, -2.6f, 2f);
        flashlight.transform.localRotation = Quaternion.Euler(86.48f, 174.31f, 180f);
        flashlight.transform.localScale = new Vector3(20, 20, 20);

        if (flashlight.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        CapsuleCollider col = flashlight.GetComponent<CapsuleCollider>();
        if (col != null)
        {
            col.enabled = false;
        }

        lightPart = flashlight.transform.Find("Light")?.gameObject;
        isFlashlightEquipped = true;

        // Desactiva texto de interacción
        interactText.gameObject.SetActive(false);

        photonView.RPC("OnFlashlightEquipped", RpcTarget.Others, flashlight.GetComponent<PhotonView>().ViewID);
    }

    private void ToggleFlashlight()
    {
        if (lightPart == null) return;

        isFlashlightOn = !isFlashlightOn;
        lightPart.SetActive(isFlashlightOn);

        photonView.RPC("OnFlashlightToggled", RpcTarget.Others, isFlashlightOn);
        Debug.Log("Linterna " + (isFlashlightOn ? "ENCENDIDA" : "APAGADA"));
    }

    private RaycastHit GetRaycastHitFromGrabPoint()
    {
        if (grabPoint == null) return default;

        if (Physics.Raycast(grabPoint.transform.position, grabPoint.transform.forward, out RaycastHit hit, interactDistance))
        {
            return hit;
        }

        return default;
    }

    [PunRPC]
    private void OnFlashlightEquipped(int viewID)
    {
        GameObject remoteFlashlight = PhotonView.Find(viewID).gameObject;
        remoteFlashlight.transform.SetParent(handPosition);
        remoteFlashlight.transform.localPosition = new Vector3(18.1f, -2.6f, 2f);
        remoteFlashlight.transform.localRotation = Quaternion.Euler(86.48f, 174.31f, 180f);
        remoteFlashlight.transform.localScale = new Vector3(20, 20, 20);
        lightPart = remoteFlashlight.transform.Find("Light")?.gameObject;
    }

    [PunRPC]
    private void OnFlashlightToggled(bool isOn)
    {
        if (lightPart == null) return;
        lightPart.SetActive(isOn);
    }
}
