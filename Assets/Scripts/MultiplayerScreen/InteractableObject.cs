using Photon.Pun;
using UnityEngine;

public class InteractableObject : MonoBehaviourPunCallbacks, IPunObservable
{
    private bool isPickedUp = false;
    private int pickedByActorNumber = -1;

    public void OnPickedUp()
    {
        if (photonView.IsMine && !isPickedUp)
        {
            isPickedUp = true;
            pickedByActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;

            // Desactivar objeto para que no se vea
            photonView.RPC("RPC_SetActiveState", RpcTarget.AllBuffered, false);
        }
    }

    [PunRPC]
    void RPC_SetActiveState(bool state)
    {
        gameObject.SetActive(state);
    }

    public void OnDropped(Vector3 dropPosition)
    {
        if (photonView.IsMine && isPickedUp)
        {
            isPickedUp = false;
            pickedByActorNumber = -1;

            // Mover el objeto a la posición de drop y activar para todos
            photonView.RPC("RPC_DropObject", RpcTarget.AllBuffered, dropPosition);
        }
    }

    [PunRPC]
    void RPC_DropObject(Vector3 pos)
    {
        transform.position = pos;
        gameObject.SetActive(true);
    }

    // Implementa IPunObservable si quieres sincronizar algún dato extra
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isPickedUp);
            stream.SendNext(pickedByActorNumber);
        }
        else
        {
            isPickedUp = (bool)stream.ReceiveNext();
            pickedByActorNumber = (int)stream.ReceiveNext();
        }
    }
}
