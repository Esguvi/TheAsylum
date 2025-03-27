using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementScript : MonoBehaviour
{
    public float velocidad = 5f;
    public float salto = 5f;
    public float gravedad = 9.81f;
    public float sensibility = 2f;
    public Transform grabPoint;
    public Transform flashLight;

    private CharacterController controller;
    private float rotVer;
    private float rotHor;
    private bool pulsado;
    private float velocidadY; 

    private Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        LockMouse();
        anim = GetComponent<Animator>();
    }

    public static void LockMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        sensibility = KeyMoConfScript.sensibility;
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

        bool enSuelo = controller.isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            velocidadY = salto;
        }

        Vector3 offset = transform.rotation * new Vector3(0, 85f, 11f);
        Camera.main.transform.position = transform.position + offset;

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 4f, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4f))
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
                    hit.rigidbody.AddForce(Camera.main.transform.forward * 1000f);
                }
            }
        }

        if (rotVer > 350 || rotVer < -350)
        {
            rotVer = 0;
        }

        // Rotación con el ratón
        float inputX = Input.GetAxis("Mouse X") * sensibility;
        rotVer += inputX;
        rotVer = Mathf.Clamp(rotVer, -360, 360);

        float inputY = Input.GetAxis("Mouse Y") * sensibility;
        rotHor -= inputY;
        rotHor = Mathf.Clamp(rotHor, -90, 90);

        transform.eulerAngles = new Vector2(0, rotVer);
        Camera.main.transform.eulerAngles = new Vector2(rotHor, rotVer);

        flashLight.position = Camera.main.transform.position;
        flashLight.rotation = Camera.main.transform.rotation;
    }

    private void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal") * velocidad;
        float ver = Input.GetAxis("Vertical") * velocidad;

        // Movimiento basado en la rotación del personaje
        Vector3 move = transform.right * hor + transform.forward * ver;

        // Manejo de la gravedad
        if (controller.isGrounded)
        {
            if (velocidadY < 0)
            {
                velocidadY = -20f; // Pequeña fuerza hacia abajo para mantener contacto con el suelo
            }
        }
        else
        {
            if (velocidadY < 0)
            {
                // Si está cayendo, aumentamos la gravedad para caer más rápido
                velocidadY -= (gravedad * 40f) * Time.deltaTime;
            }
            else
            {
                // Gravedad normal al subir
                velocidadY -= (gravedad * 40f) * Time.deltaTime;
            }
        }

        move.y = velocidadY;

        // Mover al personaje usando CharacterController
        controller.Move(move * Time.deltaTime);

        // Animaciones
        anim.SetFloat("VelX", hor);
        anim.SetFloat("VelY", ver);
    }
}
