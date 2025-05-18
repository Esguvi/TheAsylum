using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DoorScript : MonoBehaviour
{
    public GameObject puertaR;
    public GameObject puertaL;
    public AudioClip abrir;
    public AudioClip cerrar;
    
    private bool cerca;
    private bool abierto;
    private TextMeshProUGUI interactText;
    private GameObject canvas;
    private ObjectLocalizer localizer;
    private AudioSource reproductor;

    private void Start()
    {
        localizer = GetComponent<ObjectLocalizer>();
        canvas = GameObject.FindWithTag("Canva");
        interactText = canvas.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();
        reproductor = GetComponent<AudioSource>();
    }
    private void Update()
    {
        
        if (cerca)
        {
            interactText.text = localizer.GetLocalizedName();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!abierto)
                {
                    reproductor.clip = abrir;
                    reproductor.Play();
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
                    reproductor.clip = cerrar;
                    reproductor.Play();
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
        if(other.tag == "Enemy")
        {
            if (!abierto) {
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
