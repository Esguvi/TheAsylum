using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class MovementScript : MonoBehaviour
{
    public float velocidad;
    public float salto;
    public float gravedad;
    public float sensibility;
    public Transform grabPoint;
    public GameObject cameraPlayer;
    public static GameObject cameraPlayerr;

    private CharacterController controller;
    private float rotVer;
    private float rotHor;
    private bool pulsado;
    private float velocidadY;
    private float yHead;
    public int vidas = 3;

    private Animator anim;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        LockMouse();
        anim = GetComponent<Animator>();
        cameraPlayerr = cameraPlayer;
    }

    public static void LockMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        sensibility = KeyMoConfScript.sensibility;
        yHead = transform.Find("mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:Neck/mixamorig:Head").transform.position.y;

        if(this.tag == "Finish")
        {
            cameraPlayerr.GetComponent<AudioListener>().enabled = false;
            SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Single);
            OptionsBtnsScripts.backScene = true;
            GetComponent<AudioSource>().enabled = false;
        }
        
        if (SceneManager.sceneCount <= 1 && !GetComponent<AudioSource>().enabled)
        {
            GetComponent<AudioSource>().enabled = true;
        }

        if (Input.GetKey(KeyCode.Escape) && SceneManager.sceneCount == 1)
        {
            cameraPlayerr.GetComponent<AudioListener>().enabled = false;
            SceneManager.LoadScene("OptionsScreen", LoadSceneMode.Additive);
            OptionsBtnsScripts.backScene = true;
            GetComponent<AudioSource>().enabled = false; 
        }

        if (Input.GetMouseButtonDown(0))
        {
            pulsado = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            pulsado = false;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("isRunning", true);
            velocidad = 200f;
            if (Input.GetKey(KeyCode.Space))
            {
                salto = 200f;
                anim.SetBool("isRunJump", true);
            }
            else
            {
                salto = 200f;
                anim.SetBool("isRunJump", false);
            }
        }
        else
        {
            anim.SetBool("isRunning", false);
            velocidad = 100f;
        }

        //bool enSuelo = controller.isGrounded;
        bool enSuelo = Physics.Raycast(transform.position, Vector3.down, 10f);
        //bool enSuelo = Physics.CapsuleCast(transform.position, transform.position + Vector3.up * 0.1f, controller.radius, Vector3.down, 0.2f);
        //Debug.Log(enSuelo);

        if (enSuelo)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                velocidadY = salto;
                anim.SetBool("isJumping", true);
            }
            else
            {
                anim.SetBool("isJumping", false);
            }
        }
        Vector3 offset = transform.rotation * new Vector3(0,0,20f);
        cameraPlayer.transform.position = new Vector3(transform.position.x, yHead ,transform.position.z)+offset;

        Debug.DrawRay(cameraPlayer.transform.position, cameraPlayer.transform.forward * 100f, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(cameraPlayer.transform.position, cameraPlayer.transform.forward, out hit, 100f))
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
                    hit.rigidbody.AddForce(cameraPlayer.transform.forward * 1000f);
                }
            }
        }

        if (rotVer > 350 || rotVer < -350)
        {
            rotVer = 0;
        }

        float inputX = Input.GetAxis("Mouse X") * sensibility;
        rotVer += inputX;
        rotVer = Mathf.Clamp(rotVer, -360, 360);

        float inputY = Input.GetAxis("Mouse Y") * sensibility;
        rotHor -= inputY;
        rotHor = Mathf.Clamp(rotHor, -90, 90);
        //rotHor = Mathf.Clamp(rotHor, -100, 70);

        transform.eulerAngles = new Vector2(0, rotVer);
        cameraPlayer.transform.eulerAngles = new Vector2(rotHor, rotVer);

        //flashLight.position = camera.transform.position;
        //flashLight.rotation = camera.transform.rotation;
    }

    private void FixedUpdate()
    {
        float hor = Input.GetAxis("Horizontal") * velocidad;
        float ver = Input.GetAxis("Vertical") * velocidad;

        float velocidadMovimiento = new Vector3(hor, 0f, ver).magnitude;

        float velocidadNormalizada = Mathf.Clamp01(velocidadMovimiento / velocidad);

        anim.SetFloat("VelX", hor);
        anim.SetFloat("VelY", ver);

        Vector3 move = transform.right * hor + transform.forward * ver;

        if (controller.isGrounded)
        {
            if (velocidadY < 0)
            {
                velocidadY = -20f;
            }
        }
        else
        {
            if (velocidadY < 0)
            {
                velocidadY -= (gravedad * 40f) * Time.deltaTime;
            }
            else
            {
                velocidadY -= (gravedad * 40f) * Time.deltaTime;
            }
        }

        move.y = velocidadY;

        controller.Move(move * Time.deltaTime);

    }
}
