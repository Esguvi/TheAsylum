using Photon.Pun;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Transform player;
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (player != null && Input.GetMouseButtonDown(0))
        {
            var movement = player.GetComponent<MovementScript>();
            if (movement != null)
            {
                movement.vidas--;
                if (movement.vidas <= 0)
                {
                    if (photonView != null && photonView.IsMine)
                    {
                        photonView.RPC("ActualizarTag", RpcTarget.All, player.GetComponent<PhotonView>().ViewID);
                    }
                }
            }
        }
    }

    [PunRPC]
    void ActualizarTag(int viewID)
    {
        GameObject obj = PhotonView.Find(viewID).gameObject;
        if (obj != null)
        {
            obj.tag = "Finish";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy") && other.gameObject.GetComponent<MovementScript>() != null)
        {
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy") && other.gameObject.GetComponent<MovementScript>() != null)
        {
            player = null;
        }
    }
}
