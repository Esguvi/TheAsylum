using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    private Transform player;

    void Update()
    {
        if (player != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Ataque enemigo a " + player.name);
                var movement = player.GetComponent<MovementScript>();
                if (movement != null)
                {
                    movement.vidas--;
                    if (movement.vidas <= 0)
                    {
                        Debug.Log(player.name + " ha sido derrotado.");

                        PhotonView pv = player.GetComponent<PhotonView>();

                        if (pv != null && pv.IsMine)
                        {
                            StartCoroutine(DestruirJugadorYSalir(player.gameObject));
                            Debug.Log("Destruyendo jugador y saliendo a la pantalla principal...");
                        }
                    }
                }
            }
        }
    }

    private IEnumerator DestruirJugadorYSalir(GameObject playerObj)
    {
        yield return new WaitForSeconds(1.5f); 
        PhotonNetwork.Destroy(playerObj);
        SceneManager.LoadScene("MainScreen");
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
