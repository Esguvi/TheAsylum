using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject puertaR;
    public GameObject puertaL;

    private bool cerca;
    private bool abierto;
    private void Update()
    {
        if (cerca && Input.GetKeyDown(KeyCode.E))
        {
            if (!abierto)
            {
                if (puertaR != null)
                {
                    puertaR.transform.Rotate(new Vector3(0, 90f, 0));
                }
                if (puertaL != null)
                {
                    puertaL.transform.Rotate(new Vector3(0, -90f, 0));
                }
                abierto = true;
            }
            else 
            {
                if (puertaR != null)
                {
                    puertaR.transform.Rotate(new Vector3(0, -90, 0));
                }
                if (puertaL != null)
                {
                    puertaL.transform.Rotate(new Vector3(0, 90, 0));
                }
                abierto = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            cerca = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        
        if (other.gameObject.GetComponent<MovementScript>() != null)
        {
            cerca = false;
        }
    }
}
