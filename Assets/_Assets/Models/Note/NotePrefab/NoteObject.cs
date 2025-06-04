using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public string noteTitle;
    [TextArea(3, 5)] public string noteContent;
    public GameObject pickupPromptUI;

    private bool isNearNote = false;
    private NoteSystem noteSystem;
    public Camera playerCamera;

    private void Start()
    {
        if (pickupPromptUI != null)
            pickupPromptUI.SetActive(false);

        noteSystem = Object.FindFirstObjectByType<NoteSystem>(); // ? Corrected method

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

            if (noteSystem != null)
            {
                Debug.Log("Player entered note trigger."); // ? Debugging output
                noteSystem.PlayTriggerSound(noteSystem.enterTriggerSFX); // ? Play enter sound
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

            if (noteSystem != null)
            {
                Debug.Log("Player exited note trigger."); // ? Debugging output
                noteSystem.PlayTriggerSound(noteSystem.exitTriggerSFX); // ? Play exit sound
            }
        }
    }

    public void PickUpNote()
    {
        Note newNote = new Note(noteTitle, noteContent);
        Object.FindFirstObjectByType<NoteSystem>().PickUpNote(newNote); // ? Corrected method

        if (pickupPromptUI != null)
            pickupPromptUI.SetActive(false);

        //Destroy(gameObject);
    }
}