using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
            Debug.Log("Ataque enemigo a " + player.name);
            var movement = player.GetComponent<MovementScript>();
            if (movement != null)
            {
                movement.vidas--;
                if (movement.vidas <= 0)
                {
                    Debug.Log(player.name + " ha sido derrotado.");
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
            Debug.Log(obj.name + " ha sido derrotado y su tag cambiado a 'Finish'.");
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy") && other.gameObject.GetComponent<MovementScript>() != null)
        {
            player = other.transform;
            Debug.Log(other.name);
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
