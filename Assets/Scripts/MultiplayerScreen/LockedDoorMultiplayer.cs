using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using Photon.Pun;
using Unity.VisualScripting;
using System.Collections.Generic;

public class LockedDoorScriptMultiplayer : MonoBehaviourPun
{
    public GameObject puertaR;
    public GameObject puertaL;
    public GameObject puertaCerrada;
    public string keyName;

    private bool cerca;
    private bool abierto;
    private TextMeshProUGUI interactText;
    private GameObject canvas;
    private ObjectLocalizer localizer;
    private PhotonView photonView;

    private Invantory playerInventory;
    private List<GameObject> objects;
    public LocalizeStringEvent localizeEvent;

    private void Start()
    {
        localizer = GetComponent<ObjectLocalizer>();
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!cerca || interactText == null || playerInventory == null) return;

        interactText.text = localizer.GetLocalizedName();

        if (Input.GetKeyDown(KeyCode.E))
        {
            
            if (puertaCerrada.CompareTag("PuertaCerrada"))
            {
                int llaveIndex = -1;

                for (int i = 0; i < playerInventory.objectsInInvantory.Count; i++)
                {
                    Debug.Log(playerInventory.objectsInInvantory[i]);
                    if (playerInventory.objectsInInvantory[i] == null) continue;
                    var item = playerInventory.objectsInInvantory[i].itemLogic;

                    if (item != null && item.name == "Llave Final")
                    {
                        PhotonNetwork.LoadLevel("WinScreen");
                        return;
                    }

                    if (item != null && item.name == keyName)
                    {
                        llaveIndex = i;
                        break;
                    }
                }

                if (llaveIndex >= 0)
                {
                    GameObject llaveGO = objects[llaveIndex];
                    PhotonView itemPV = llaveGO.GetComponent<PhotonView>();

                    if (itemPV != null && itemPV.IsMine)
                    {
                        PhotonNetwork.Destroy(llaveGO);
                        objects.RemoveAt(llaveIndex);
                    }

                    playerInventory.UseItemAtID(llaveIndex);

                    photonView.RPC("AbrirPuerta", RpcTarget.AllBuffered);
                    photonView.RPC("ActualizarLocalizacion", RpcTarget.AllBuffered);
                }
            }
            else if (puertaCerrada.CompareTag("PuertaAbierta"))
            {
                photonView.RPC("AbrirPuerta", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    private void AbrirPuerta()
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

    [PunRPC]
    private void ActualizarLocalizacion()
    {
        puertaCerrada.tag = "PuertaAbierta";
        localizer.localizedObjectName.TableEntryReference = "door";
        localizeEvent.StringReference.TableEntryReference = "door";
        localizeEvent.RefreshString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !abierto)
        {
            photonView.RPC("AbrirPuerta", RpcTarget.AllBuffered);
            return;
        }

        if (!other.CompareTag("Player")) return;

        PhotonView playerView = other.GetComponent<PhotonView>();
        if (playerView != null && playerView.IsMine)
        {
            cerca = true;

            canvas = other.transform.Find("Canvas")?.gameObject;
            interactText = canvas?.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();
            playerInventory = other.transform.Find("InvantoryBar").GetComponent<Invantory>();
            objects = other.GetComponent<ObjectInteractableMultiplayer>().objectsInInventory;

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
            photonView.RPC("AbrirPuerta", RpcTarget.AllBuffered);
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
