using JetBrains.Annotations;
using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    public MovementScript movement;
    public GameObject camera;

    public void IsLocalPlayer()
    {
        movement.enabled = true;
        camera.SetActive(true);
    }

}
