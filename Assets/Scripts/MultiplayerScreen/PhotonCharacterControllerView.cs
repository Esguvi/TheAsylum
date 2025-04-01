using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Photon Networking/Photon CharacterController View")]
public class PhotonCharacterControllerView : MonoBehaviourPun, IPunObservable
{
    private CharacterController characterController;
    private Vector3 networkPosition;
    private Quaternion networkRotation;

    private float distance;
    private float angle;

    [HideInInspector]
    public bool teleportEnabled = false;
    [HideInInspector]
    public float teleportIfDistanceGreaterThan = 3.0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        networkPosition = transform.position;
        networkRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            // Si la distancia es demasiado grande, hacer un teletransporte
            if (teleportEnabled && Vector3.Distance(transform.position, networkPosition) > teleportIfDistanceGreaterThan)
            {
                characterController.enabled = false; // Desactivamos momentáneamente para evitar bugs
                transform.position = networkPosition;
                characterController.enabled = true;
            }

            // Interpolación para movimiento fluido
            transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 10f);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * 10f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // El jugador local envía su posición y rotación
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Los otros jugadores reciben la posición y rotación
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
