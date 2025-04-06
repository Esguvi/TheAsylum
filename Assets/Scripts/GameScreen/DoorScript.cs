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
    private bool enemy;
    private TextMeshProUGUI interactText;
    private GameObject canvas;
    private ObjectLocalizer localizer;

    private void Start()
    {
        localizer = GetComponent<ObjectLocalizer>();
        canvas = GameObject.FindWithTag("Canva");
        interactText = canvas.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        interactText.text = localizer.GetLocalizedName();
        if (cerca || enemy)
        {
            if (Input.GetKeyDown(KeyCode.E) || enemy)
            {
                enemy = false;
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
        Debug.Log(other.tag);
        if(other.tag == "Enemy")
        {
            Debug.Log("ABRIR");
            enemy = true;
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
            enemy = true;
        }

        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            cerca = false;
            ObjectInteractableSolo.showDoorText = false;
        }
    }
}
