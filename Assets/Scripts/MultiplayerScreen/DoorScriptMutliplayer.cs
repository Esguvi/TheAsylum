using UnityEngine;
using TMPro;
using Photon.Pun;

public class DoorScriptMultiplayer : MonoBehaviourPun
{
    public GameObject puertaR;
    public GameObject puertaL;

    private bool cerca;
    private bool abierto;
    private TextMeshProUGUI interactText;
    private GameObject canvas;
    private ObjectLocalizer localizer;

    private PhotonView photonView;

    private void Start()
    {
        localizer = GetComponent<ObjectLocalizer>();
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!cerca || !photonView.IsMine) return;

        if (interactText != null)
        {
            interactText.text = localizer.GetLocalizedName();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            photonView.RPC("ToggleDoor", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void ToggleDoor()
    {
        if (!abierto)
        {
            puertaR?.transform.Rotate(0, 90f, 0);
            puertaL?.transform.Rotate(0, -90f, 0);
            abierto = true;
        }
        else
        {
            puertaR?.transform.Rotate(0, -90f, 0);
            puertaL?.transform.Rotate(0, 90f, 0);
            abierto = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !abierto)
        {
            photonView.RPC("ToggleDoor", RpcTarget.AllBuffered);
            return;
        }

        if (!other.CompareTag("Player")) return;

        PhotonView playerView = other.GetComponent<PhotonView>();
        if (playerView != null && playerView.IsMine)
        {
            cerca = true;

            canvas = other.transform.Find("Canvas")?.gameObject;
            interactText = canvas?.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();

            ObjectInteractableMultiplayer.showDoorText = true;

            if (interactText != null)
            {
                ObjectInteractableMultiplayer.externalInteractText = localizer.GetLocalizedName();
                interactText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && abierto)
        {
            photonView.RPC("ToggleDoor", RpcTarget.AllBuffered);
            return;
        }

        if (!other.CompareTag("Player")) return;

        PhotonView playerView = other.GetComponent<PhotonView>();
        if (playerView != null && playerView.IsMine)
        {
            cerca = false;

            canvas = other.transform.Find("Canvas")?.gameObject;
            interactText = canvas?.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();

            ObjectInteractableMultiplayer.showDoorText = false;

            if (interactText != null)
            {
                interactText.gameObject.SetActive(false);
                ObjectInteractableMultiplayer.externalInteractText = "";
            }
        }
    }
}