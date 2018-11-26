using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonController : MonoBehaviour {

    public LayerMask groundLayers;
    public float walkingSpeed = 2.0f;
    public float runningSpeed = 4.0f;
    public float jumpHeight = 1f;
    public bool isPlayerActive;

    [SerializeField] List<SecurityCamController> securityCamControllers = new List<SecurityCamController>();
    GameObject[] InfiltrationGOArray;
    CapsuleCollider col;
    Rigidbody rb;
    Camera fpsCam;
    float speed = 2.0f;
    [SerializeField] float raycastRange = 100f;

    // Use this for initialization
    void Start ()
    {
        InfiltrationGOArray = SceneManager.GetSceneByName("Infiltration").GetRootGameObjects();
        speed = walkingSpeed;
        isPlayerActive = true;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        fpsCam = GetComponentInChildren<Camera>();

        foreach( GameObject go in InfiltrationGOArray )
        {
            if( go.name == "SecurityCam" )
            {
                securityCamControllers.Add(go.GetComponent<SecurityCamController>());
            }
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        if( isPlayerActive )
        {
            RaycastHit hit = (RaycastHit)FireRaycast();
            if( Input.GetKeyDown(KeyCode.Mouse0) && hit.transform.name == "Cam1Feed" && securityCamControllers[0] )
            {
                Cursor.lockState = CursorLockMode.Locked;
                isPlayerActive = false;
                securityCamControllers[0].isCamActive = true;
                return;
            }
            print(hit.transform.name);

            if (Input.GetKeyDown(KeyCode.Escape)) { Cursor.lockState = CursorLockMode.None; }
            if (Input.GetKeyDown(KeyCode.Mouse0)) { Cursor.lockState = CursorLockMode.Locked; }
            if (Input.GetKey(KeyCode.LeftShift)) { speed = runningSpeed; }
            if (Input.GetKeyUp(KeyCode.LeftShift)) { speed = walkingSpeed; }

            float forward = Input.GetAxis("Vertical");
            float strafe = Input.GetAxis("Horizontal");

            forward *= Time.deltaTime * speed;
            strafe *= Time.deltaTime * speed;

            transform.Translate(strafe, 0, forward);

            if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }
        }
    }

    RaycastHit? FireRaycast()
    {
        RaycastHit hit;
        if( Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, raycastRange) )
        {
            return hit;
        }
        else
        {
            return null;
        }
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * 0.1f, groundLayers);
    }
}
