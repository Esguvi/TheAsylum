using UnityEngine;

public class NoteObjectMultiplayer : MonoBehaviour
{
    public string noteTitle;
    [TextArea(3, 5)] public string noteContent;
    public GameObject pickupPromptUI;

    private bool isNearNote = false;
    private NoteSystemMultiplayer noteSystemMultiplayer;
    public Camera playerCamera;

    private void Start()
    {
        if (pickupPromptUI != null)
            pickupPromptUI.SetActive(false);

        noteSystemMultiplayer = Object.FindFirstObjectByType<NoteSystemMultiplayer>(); // ? Corrected method

        // ? Get camera reference (Handles cases where Camera.main is disabled)
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            Debug.LogWarning("No active camera found! Searching for any available camera...");
            playerCamera = Object.FindFirstObjectByType<Camera>(); // ? Corrected method
        }
    }

    private void Update()
    {
        // ? Ensure the prompt follows the camera
        if (pickupPromptUI != null && playerCamera != null)
        {
            pickupPromptUI.transform.LookAt(playerCamera.transform);
            pickupPromptUI.transform.Rotate(0, 180, 0); // Flip the UI to face correctly
        }

        if (isNearNote && Input.GetKeyDown(KeyCode.E))
        {
            PickUpNote();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearNote = true;
            if (pickupPromptUI != null)
                pickupPromptUI.SetActive(true);

            if (noteSystemMultiplayer != null)
            {
                Debug.Log("Player entered note trigger."); // ? Debugging output
                noteSystemMultiplayer.PlayTriggerSound(noteSystemMultiplayer.enterTriggerSFX); // ? Play enter sound
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearNote = false;
            if (pickupPromptUI != null)
                pickupPromptUI.SetActive(false);

            if (noteSystemMultiplayer != null)
            {
                Debug.Log("Player exited note trigger."); // ? Debugging output
                noteSystemMultiplayer.PlayTriggerSound(noteSystemMultiplayer.exitTriggerSFX); // ? Play exit sound
            }
        }
    }

    public void PickUpNote()
    {
        NoteData newNote = new NoteData(noteTitle, noteContent);
        Object.FindFirstObjectByType<NoteSystemMultiplayer>().PickUpNote(newNote); // ? Corrected method

        if (pickupPromptUI != null)
            pickupPromptUI.SetActive(false);

        //Destroy(gameObject);
    }
}
