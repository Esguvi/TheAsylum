using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public MovementScript movement;
    public GameObject playerCamera;

    public void IsLocalPlayer()
    {
        movement.enabled = true;
        playerCamera.SetActive(true);
    }
}