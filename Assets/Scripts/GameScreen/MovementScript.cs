using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI.Table;

public class MovementScript : MonoBehaviour
{
    public float velocidad;
    public float salto;
    public Transform grabPoint;
    public Transform flashLight;

    private Rigidbody rb;
    private float rotVer;
    private float rotHor;
    private bool pulsado;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        LockMouse();
    }

    public static void LockMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;   
    }

    private void Update()
    {
        Debug.Log(KeyMoConfScript.sensibility);
        if (Input.GetKey(KeyCode.Tab) && SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
            OptionsBtnsScripts.backScene = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            pulsado = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            pulsado = false;
        }
        bool salto = Physics.Raycast(transform.position, Vector3.down, 2f);
        if (Input.GetKeyDown(KeyCode.Space) && salto)
        {
            Jump();
        }
        Camera.main.transform.position = new Vector3(
                                                transform.position.x,
                                                transform.position.y + 0.7f,
                                                transform.position.z);

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.transform.forward * 4f, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.transform.forward, out hit, 4f))
        {
            if (hit.rigidbody != null)
            {
                if (pulsado)
                {
                    hit.rigidbody.useGravity = false;
                    hit.rigidbody.MovePosition(grabPoint.position);

                }
                else
                {
                    hit.rigidbody.useGravity = true;
                }
                if (Input.GetMouseButtonDown(1))
                {
                    hit.rigidbody.useGravity = true;
                    pulsado = false;
                    hit.rigidbody.AddForce(Camera.main.transform.transform.forward * 1000f);
                }

            }

        }


        float inputX = Input.GetAxis("Mouse X") * KeyMoConfScript.sensibility;

        rotVer += inputX;
        if (rotVer > 350 || rotVer < -350)
        {
            rotVer = 0;
        }
        rotVer = Mathf.Clamp(rotVer, -360, 360);

        float inputY = Input.GetAxis("Mouse Y") * KeyMoConfScript.sensibility;

        rotHor -= inputY;

        rotHor = Mathf.Clamp(rotHor, -90, 90);

        transform.eulerAngles = new Vector2(0, rotVer);

        Camera.main.transform.eulerAngles = new Vector2(rotHor, rotVer);

        flashLight.position = Camera.main.transform.position;
        flashLight.rotation = Camera.main.transform.rotation;
    }
    private void FixedUpdate()
    {
        Vector3 rotacion = new Vector3(0, 90, 0);
        float ver;
        float hor = Input.GetAxis("Horizontal") * velocidad;
        if (transform.rotation.y < 90 && transform.rotation.y > -90)
        {
            ver = Input.GetAxis("Vertical") * velocidad;
        }
        else ver = Input.GetAxis("Vertical") * velocidad * -1f;
        Vector3 direccion = new Vector3(hor, 0, ver);
        transform.Translate(direccion);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * salto);
    }
}