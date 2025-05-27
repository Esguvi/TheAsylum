using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class LockedDoorScript : MonoBehaviour
{
    public GameObject puertaR;
    public GameObject puertaL;
    public Invantory playerInventory;
    private bool cerca;
    private bool abierto;
    public GameObject puertaCerrada;
    private TextMeshProUGUI interactText;
    private GameObject canvas;
    private ObjectLocalizer localizer;
    public LocalizeStringEvent localizeEvent;

    private void Start()
    {
        localizer = GetComponent<ObjectLocalizer>();
        canvas = GameObject.FindWithTag("Canva");
        interactText = canvas.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
            if (cerca)
            {
                interactText.gameObject.SetActive(true);
                interactText.text = localizer.GetLocalizedName();
                Debug.Log("Cerca de la puerta: " + localizer.GetLocalizedName());

            if (puertaCerrada.tag == "PuertaCerrada" )
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {

                        int llaveIndex = -1;

                        for (int i = 0; i < playerInventory.objectsInInvantory.Count; i++)
                        {
                            if (playerInventory.objectsInInvantory[i] != null &&
                                playerInventory.objectsInInvantory[i].itemLogic != null &&
                                playerInventory.objectsInInvantory[i].itemLogic.name == "Llave")
                            {
                                llaveIndex = i;
                                Debug.Log("Llave encontrada en el inventario." + llaveIndex);
                                break;
                            }
                        }
                        Debug.LogError("LlaveIndex: " + llaveIndex);
                        if (llaveIndex >= 0)
                        {
                            Debug.Log("Llave encontrada.");
                            playerInventory.UseItemAtID(llaveIndex);
                            abrirPuertas();
                            puertaCerrada.tag = "PuertaAbierta";
                            localizer.localizedObjectName.TableEntryReference = "door";
                            localizeEvent.StringReference.TableEntryReference = "door";                   
                            localizeEvent.RefreshString();

                        }
                        else
                        {
                            Debug.Log("No tienes una llave.");
                        }

                    }

                }
                else if (puertaCerrada.tag == "PuertaAbierta")
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        abrirPuertas();
                    }
            }    
    }

    private void abrirPuertas()
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