using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ObjectPickup : MonoBehaviourPun
{
    public GameObject interactableObject;
    public GameObject lightObject;

    public bool IsEquipped { get; private set; } = false;

    private void Start()
    {
        if (!photonView.IsMine)
            return;

        if (lightObject != null)
            lightObject.SetActive(false);
    }

    public void Equip()
    {
        if (!photonView.IsMine)
            photonView.RequestOwnership();

        IsEquipped = true;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject owner = null;

        foreach (var p in players)
        {
            PhotonView view = p.GetComponent<PhotonView>();
            if (view != null && view.Owner == photonView.Owner)
            {
                owner = p;
                break;
            }
        }

        if (owner == null)
        {
            Debug.LogWarning("No se encontró el jugador dueño de este objeto.");
            return;
        }

        Transform handBone = owner.transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand/mixamorig:LeftHandMiddle1");

        if (handBone == null)
        {
            Debug.LogWarning("No se encontró el hueso de la mano.");
            return;
        }

        transform.SetParent(handBone);
        transform.localPosition = new Vector3(-0.0628f, 0.0764f, 0.1323f);
        transform.localRotation = Quaternion.Euler(11.205f, 215.8f, 86.433f);
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        if (TryGetComponent<Collider>(out var col))
            col.enabled = false;

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;

        if (lightObject != null)
            lightObject.SetActive(false);
    }

    public void Toggle()
    {
        if (!IsEquipped || lightObject == null) return;

        bool newState = !lightObject.activeSelf;
        lightObject.SetActive(newState);

        Debug.Log("Objeto " + (newState ? "ENCENDIDO" : "APAGADO"));
    }
}
