using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject puertaR;
    public GameObject puertaL;
    public bool rotate;

    private int Rx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            if (puertaR != null)
            {
                if (puertaR.transform.rotation.x != 0)
                {
                    Rx = 180;
                }
                else
                {
                    Rx = 0;
                }
                //puertaR.transform.rotation = new Quaternion(Rx, 0, 0, 0);
                puertaR.transform.Rotate(new Vector3(0,90f,0));
            }
            if (puertaL != null)
            {
                //puertaL.transform.rotation = new Quaternion(0, 0, 0, 0);
                puertaL.transform.Rotate(new Vector3(0, -90f, 0));
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        
        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            if (puertaR != null)
            {
                puertaR.transform.Rotate(new Vector3(0, -90, 0));
            }
            if (puertaL != null)
            {
                puertaL.transform.Rotate(new Vector3(0, 90, 0));
            }
        }
    }
}
