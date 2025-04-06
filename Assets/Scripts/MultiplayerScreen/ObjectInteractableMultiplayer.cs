using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ObjectInteractableMultiplayer : MonoBehaviourPunCallbacks
{
    private TextMeshProUGUI interactText;
    private ObjectLocalizer localizer;

    private GameObject grabPoint;
    private Transform handPosition;
    private GameObject flashLight;
    private GameObject lightt;

    private bool isFlashlightEquipped = false;
    private bool isFlashlightOn = false;

    private float interactDistance = 100f;
    private bool playerInitialized = false;

    void Awake()
    {
        localizer = GetComponent<ObjectLocalizer>();
    }

    void Start()
    {
        TryFindPlayerComponents();
    }

    void Update()
    {
        if (!playerInitialized)
        {
            TryFindPlayerComponents();
            return;
        }

        if (!photonView.IsMine) return;

        RaycastHit hit = GetRaycastHitFromGrabPoint();

        if (hit.collider != null && hit.collider.CompareTag("FlashLight"))
        {
            interactText.text = $"{localizer?.GetLocalizedName()}";
            interactText.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                EquipFlashLight();
                //PhotonNetwork.Destroy(gameObject);
                Debug.Log("Linterna destruida");
            }
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            lightt = transform.Find("Light").gameObject;
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

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }

        if (handPosition == null || grabPoint == null || interactText == null)
        {
            Debug.LogError("No se encontraron todos los componentes del Player.");
            return;
        }

        flashLight = gameObject;

        LocalizeStringEvent localizeEvent = flashLight.GetComponent<LocalizeStringEvent>();
        if (localizeEvent == null)
        {
            localizeEvent = flashLight.AddComponent<LocalizeStringEvent>();
        }

        localizeEvent.StringReference.SetReference("StringTable", "flashlight");

        localizeEvent.OnUpdateString.AddListener((localizedText) =>
        {
            if (interactText != null)
            {
                interactText.text = localizedText;
            }
        });

        playerInitialized = true;
        Debug.Log("Player encontrado y referencias asignadas correctamente.");
    }

    private void EquipFlashLight()
    {
        if (handPosition != null)
        {
            if (flashLight == null)
            {
                flashLight = PhotonNetwork.Instantiate("FlashLight", Vector3.zero, Quaternion.identity);
            }

            flashLight.transform.SetParent(handPosition);
            flashLight.transform.localPosition = new Vector3(18.1f, -2.6f, 2f);
            flashLight.transform.localRotation = Quaternion.Euler(86.48f, 174.31f, 180f);
            flashLight.transform.localScale = new Vector3(20, 20, 20);

            if (flashLight.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }

            isFlashlightEquipped = true;

            photonView.RPC("OnFlashlightEquipped", RpcTarget.Others, flashLight.GetComponent<PhotonView>().ViewID);

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
        if (lightt == null) return;

        isFlashlightOn = !isFlashlightOn;
        lightt.SetActive(isFlashlightOn);
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
    private void OnFlashlightEquipped(int flashlightViewID)
    {
        PhotonView flashlightPhotonView = PhotonView.Find(flashlightViewID);
        if (flashlightPhotonView != null)
        {
            flashLight = flashlightPhotonView.gameObject;
            flashLight.transform.SetParent(handPosition);
            flashLight.transform.localPosition = new Vector3(18.1f, -2.6f, 2f);
            flashLight.transform.localRotation = Quaternion.Euler(86.48f, 174.31f, 180f);
            flashLight.transform.localScale = new Vector3(20, 20, 20);
        }
    }

    [PunRPC]
    private void OnFlashlightToggled(bool isOn)
    {
        if (flashLight == null) return;
        flashLight.SetActive(isOn);
    }
}