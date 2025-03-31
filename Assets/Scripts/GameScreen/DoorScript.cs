using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject puertaR;
    public GameObject puertaL;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            puertaR.transform.rotation = new Quaternion(0, -90, 0, -90);
            puertaL.transform.rotation = new Quaternion(0, 90, 0, -90);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            puertaR.transform.rotation = new Quaternion(0, 0, 0, 0);
            puertaL.transform.rotation = new Quaternion(0, 0, 0, 0);
            Debug.Log("CERRAR");
        }
    }
}
