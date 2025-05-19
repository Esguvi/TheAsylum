using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.SocialPlatforms.Impl;

public class LockedDoorScript : MonoBehaviour
{
    public GameObject puertaR;
    public GameObject puertaL;
    public Invantory playerInventory;
    public GameObject grabPoint;
    private bool cerca;
    private bool abierto;
    public GameObject puertaCerrada;
    private TextMeshProUGUI interactText;
    private GameObject canvas;
    private ObjectLocalizer localizer;
    public LocalizeStringEvent localizeEvent;
    RaycastHit hit;
    private float interactDistance = 3f;

    private void Start()
    {
        localizer = GetComponent<ObjectLocalizer>();
        canvas = GameObject.FindWithTag("Canva");
        interactText = canvas.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();
    }



    void Awake()
    {
        if (puertaL == null)
            puertaL = transform.Find("RotationLeft")?.gameObject;

        if (puertaR == null)
            puertaR = transform.Find("RotationRight")?.gameObject;
    }
    private void Update()
    {
        hit = GetRaycastHitFromGrabPoint();

        if (hit.collider != null)
        {
            string tag = hit.collider.tag;
            if (cerca)
            {
                if (puertaCerrada.tag == "PuertaCerrada")
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ObjectLocalizer hitLocalizer = hit.collider.GetComponent<ObjectLocalizer>();
                        if (hitLocalizer != null)
                        {
                            interactText.text = hitLocalizer.GetLocalizedName();
                            interactText.gameObject.SetActive(true);
                        }


                        int llaveIndex = -1;

                        for (int i = 0; i < playerInventory.objectsInInvantory.Count; i++)
                        {
                            if (playerInventory.objectsInInvantory[i] != null &&
                                playerInventory.objectsInInvantory[i].itemLogic != null &&
                                playerInventory.objectsInInvantory[i].itemLogic.name == "Llave")
                            {
                                llaveIndex = i;
                                break;
                            }
                        }

                        if (llaveIndex >= 0)
                        {
                            Debug.Log("Llave encontrada.");
                            playerInventory.UseItemAtID(llaveIndex);
                            abrirPuertas();
                            puertaCerrada.tag = "PuertaAbierta";
                            localizeEvent.StringReference.TableEntryReference = "puertaAbierta";
                            localizeEvent.RefreshString();
                        }
                        else
                        {
                            Debug.Log("No tienes una llave.");
                        }


                    }



                }
                else if (puertaCerrada.tag == "PuertaAbierta")

                    abrirPuertas();
                    localizeEvent.StringReference.TableEntryReference = "puertaAbierta";
                    localizeEvent.RefreshString();
            }
        }
    }
    private RaycastHit GetRaycastHitFromGrabPoint()
    {
        if (Physics.Raycast(grabPoint.transform.position, grabPoint.transform.forward, out RaycastHit hit, interactDistance))
        {
            return hit;
        }

        return default;
    }
    private void abrirPuertas()
    {
        if (cerca)
        {
            interactText.text = localizer.GetLocalizedName();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!abierto)
                {
                    if (puertaR != null)
                    {
                        puertaR.transform.Rotate(new Vector3(0, 90f, 0));
                    }
                    if (puertaL != null)
                    {
                        puertaL.transform.Rotate(new Vector3(0, -90f, 0));
                    }
                    abierto = true;
                }
                else
                {
                    if (puertaR != null)
                    {
                        puertaR.transform.Rotate(new Vector3(0, -90, 0));
                    }
                    if (puertaL != null)
                    {
                        puertaL.transform.Rotate(new Vector3(0, 90, 0));
                    }
                    abierto = false;
                }
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (!abierto)
            {
                if (puertaR != null)
                {
                    puertaR.transform.Rotate(new Vector3(0, 90f, 0));
                }
                if (puertaL != null)
                {
                    puertaL.transform.Rotate(new Vector3(0, -90f, 0));
                }
                abierto = true;
            }
        }

        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            cerca = true;
            ObjectInteractableSolo.showDoorText = true;
            interactText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Enemy")
        {
            if (abierto)
            {
                if (puertaR != null)
                {
                    puertaR.transform.Rotate(new Vector3(0, -90, 0));
                }
                if (puertaL != null)
                {
                    puertaL.transform.Rotate(new Vector3(0, 90, 0));
                }
                abierto = false;
            }
        }

        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            cerca = false;
            ObjectInteractableSolo.showDoorText = false;
        }
    }
}