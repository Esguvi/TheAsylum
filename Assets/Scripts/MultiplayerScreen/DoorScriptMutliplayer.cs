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

    private void Start()
    {
        localizer = GetComponent<ObjectLocalizer>();
    }

    private void Update()
    {
        if (cerca)
        {
            if (interactText != null)
            {
                interactText.text = localizer.GetLocalizedName();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Tecla E pulsada");

                photonView.RPC("ToggleDoor", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    void ToggleDoor()
    {
        if (!abierto)
        {
            if (puertaR != null) puertaR.transform.Rotate(new Vector3(0, 90f, 0));
            if (puertaL != null) puertaL.transform.Rotate(new Vector3(0, -90f, 0));
            abierto = true;
        }
        else
        {
            if (puertaR != null) puertaR.transform.Rotate(new Vector3(0, -90f, 0));
            if (puertaL != null) puertaL.transform.Rotate(new Vector3(0, 90f, 0));
            abierto = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!abierto)
            {
                photonView.RPC("ToggleDoor", RpcTarget.AllBuffered);
            }
        }

        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            cerca = true;
            canvas = other.transform.Find("Canvas")?.gameObject;
            interactText = canvas?.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();

            if (interactText != null)
            {
                ObjectInteractableSolo.showDoorText = true;
                interactText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (abierto)
            {
                photonView.RPC("ToggleDoor", RpcTarget.AllBuffered);
            }
        }

        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            cerca = false;

            canvas = other.transform.Find("Canvas")?.gameObject;
            interactText = canvas?.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();

            if (interactText != null)
            {
                ObjectInteractableSolo.showDoorText = false;
                interactText.gameObject.SetActive(false);
            }
        }
    }
}
