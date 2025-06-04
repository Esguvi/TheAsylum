using UnityEngine;
using TMPro;

public class NoteSystem : MonoBehaviour
{
    public GameObject noteUI;
    public TMP_Text noteTitleText;
    public TMP_Text noteContentText;
    public GameObject exitButton;
    public AudioSource audioSource;
    public AudioClip pickUpSFX;
    public AudioClip closeSFX;
    public AudioClip enterTriggerSFX;
    public AudioClip exitTriggerSFX;
    public MonoBehaviour playerMovementScript; // ? Reference to player movement script

    private Note currentNote;
    private bool isNoteOpen = false;
    private bool canPlayTriggerSound = true; // ? Cooldown for entry/exit sounds
    private float triggerSoundCooldown = 0.5f; // ? Adjust cooldown duration

    private void Start()
    {
        if (noteUI != null)
            noteUI.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    private void Update()
    {
        // ? Allow Escape key to close the note
        if (isNoteOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseNote();
        }
    }

    public void PickUpNote(Note note)
    {
        if (audioSource && pickUpSFX)
            audioSource.PlayOneShot(pickUpSFX);

        currentNote = note;
        ShowNote();
    }

    public void ShowNote()
    {
        Debug.Log("Mostrando nota: " + currentNote.title);

        if (currentNote == null) return;

        noteTitleText.text = currentNote.title;
        noteContentText.text = currentNote.content;
        noteUI.SetActive(true);
        currentNote.isRead = true;
        isNoteOpen = true;

        // ? Pause game and disable ONLY player movement (NOT camera)
        Time.timeScale = 0f;
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        // ? Show and unlock mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        if (audioSource && closeSFX)
            audioSource.PlayOneShot(closeSFX);

        noteUI.SetActive(false);
        isNoteOpen = false;

        // ? Unpause game and re-enable player movement
        Time.timeScale = 1f;
        if (playerMovementScript != null)
            playerMovementScript.enabled = true;

        // ? Hide and lock mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void NextNote()
    {
        Debug.Log("Next note functionality not implemented yet.");
    }

    // ? Play sounds for entering and exiting the trigger (with cooldown)
    public void PlayTriggerSound(AudioClip clip)
    {
        if (canPlayTriggerSound && audioSource && clip)
        {
            audioSource.PlayOneShot(clip);
            canPlayTriggerSound = false;
            Invoke(nameof(ResetTriggerSound), triggerSoundCooldown);
        }
    }

    private void ResetTriggerSound()
    {
        canPlayTriggerSound = true;
    }
}

[System.Serializable]
public class Note
{
    public string title;
    public string content;
    public bool isRead;

    public Note(string title, string content)
    {
        this.title = title;
        this.content = content;
        this.isRead = false;
    }
}