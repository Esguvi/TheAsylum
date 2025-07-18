using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.SceneManagement;

public class LockedDoorScript : MonoBehaviour
{
    public GameObject puertaR;
    public GameObject puertaL;
    public Invantory playerInventory;
    public LocalizeStringEvent localizeEvent;
    public GameObject puertaCerrada;
    public string keyName;
    public AudioClip audioAbrir;
    public AudioClip audioCerrar;

    private bool cerca;
    private bool abierto;
    private TextMeshProUGUI interactText;
    private GameObject canvas;
    private ObjectLocalizer localizer;
    private AudioSource audioSource;

    private void Start()
    {
        localizer = GetComponent<ObjectLocalizer>();
        canvas = GameObject.FindWithTag("Canva");
        interactText = canvas.transform.Find("InteractionText")?.GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        gameObject.GetComponent<AudioSource>().volume = AudioConfScript.volume;
        if (cerca)
        {
            interactText.gameObject.SetActive(true);
            interactText.text = localizer.GetLocalizedName();

            if (puertaCerrada.tag == "PuertaCerrada")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    int llaveIndex = -1;

                    for (int i = 0; i < playerInventory.objectsInInvantory.Count; i++)
                    {
                        if (playerInventory.objectsInInvantory[i].itemLogic.name.Equals("Llave Final") && this.name.Equals("MainDoor"))
                        {
                            SceneManager.LoadScene("WinScreen"); 
                        }
                        if (playerInventory.objectsInInvantory[i] != null &&
                            playerInventory.objectsInInvantory[i].itemLogic != null &&
                            playerInventory.objectsInInvantory[i].itemLogic.name == keyName)
                        {
                            llaveIndex = i;
                            break;
                        }
                    }
                    if (llaveIndex >= 0)
                    {
                        playerInventory.UseItemAtID(llaveIndex);
                        Destroy(ObjectInteractableSolo.objectsInInventory[llaveIndex]);
                        ObjectInteractableSolo.objectsInInventory.RemoveAt(llaveIndex);
                        abrirPuertas();
                        puertaCerrada.tag = "PuertaAbierta";
                        localizer.localizedObjectName.TableEntryReference = "door";
                        localizeEvent.StringReference.TableEntryReference = "door";
                        localizeEvent.RefreshString();

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
            audioSource.clip = audioAbrir;
            audioSource.Play();
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
            audioSource.clip = audioCerrar;
            audioSource.Play();
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