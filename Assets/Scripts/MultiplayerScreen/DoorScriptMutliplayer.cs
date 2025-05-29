using UnityEngine;
using TMPro;
using Photon.Pun;

public class DoorScriptMultiplayer : MonoBehaviourPun
{
    public GameObject puertaR;
    public GameObject puertaL;

    private bool abierto;
    private ObjectLocalizer localizer;

    private void Start()
    {
        localizer = GetComponent<ObjectLocalizer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView otherPhotonView = other.GetComponent<PhotonView>();

        if (other.CompareTag("Enemy") && !abierto)
        {
            photonView.RPC("ToggleDoor", RpcTarget.AllBuffered);
        }

        if (other.TryGetComponent<PhotonView>(out var pv) && pv.IsMine && other.GetComponent<MovementScript>() != null)
        {
            Transform canvas = other.transform.Find("Canvas");
            TextMeshProUGUI interactText = canvas?.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();

            if (interactText != null)
            {
                interactText.text = localizer.GetLocalizedName();
                interactText.gameObject.SetActive(true);
                StartCoroutine(WaitForInteractionKey(other.transform, interactText));
            }
        }

    }

    private System.Collections.IEnumerator WaitForInteractionKey(Transform playerTransform, TextMeshProUGUI interactText)
    {
        while (Vector3.Distance(playerTransform.position, transform.position) < 3f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Tecla E pulsada");
                photonView.RPC("ToggleDoor", RpcTarget.AllBuffered);
                break;
            }
            yield return null;
        }

        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        PhotonView otherPhotonView = other.GetComponent<PhotonView>();

        if (other.CompareTag("Enemy") && abierto)
        {
            photonView.RPC("ToggleDoor", RpcTarget.AllBuffered);
        }

        if (other.TryGetComponent<PhotonView>(out var pv) && pv.IsMine && other.GetComponent<MovementScript>() != null)
        {
            Transform canvas = other.transform.Find("Canvas");
            TextMeshProUGUI interactText = canvas?.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();

            if (interactText != null)
            {
                interactText.gameObject.SetActive(false);
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
}
