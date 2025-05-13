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
        {
            return;
        }

        if (lightObject != null)
        {
            lightObject.SetActive(false);
        }
    }

    public void Equip(Transform hand)
    {
        if (!photonView.IsMine)
            photonView.RequestOwnership();

        IsEquipped = true;

        transform.SetParent(hand);
        transform.localPosition = new Vector3(18.1f, -2.6f, 2f);
        transform.localRotation = Quaternion.Euler(86.48f, 174.31f, 180f);
        transform.localScale = new Vector3(20, 20, 20);

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
