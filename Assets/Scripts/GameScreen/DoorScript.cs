using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DoorScript : MonoBehaviour
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
            else
            {
                Debug.Log("NO LO ENCUENTRA");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E KEY");
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
            Debug.Log(cerca);
            cerca = true;
            Debug.Log(cerca);

            canvas = other.transform.Find("Canvas")?.gameObject;
            interactText = canvas?.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();

            ObjectInteractableSolo.showDoorText = true;

            if (interactText != null)
            {
                interactText.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("NO LO ENCUENTRA");
            }
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

            canvas = other.transform.Find("Canvas")?.gameObject;
            interactText = canvas?.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();

            ObjectInteractableSolo.showDoorText = false;

            if (interactText != null)
            {
                interactText.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("NO LO ENCUENTRA");
            }
        }
    }
}