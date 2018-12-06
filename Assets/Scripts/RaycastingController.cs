using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class RaycastingController : MonoBehaviour {
    
    public bool isPlayerActive;

    [SerializeField] List<SecurityCamController> securityCamControllers = new List<SecurityCamController>();
    [SerializeField] readonly float raycastRange = 2.0f;

    GameObject[] InfiltrationGOArray;
    CapsuleCollider col;
    Rigidbody rb;
    Camera fpsCam;
    float speed = 2.0f;
    private FirstPersonController fpsController;

    // Use this for initialization
    void Start ()
    {
        InfiltrationGOArray = SceneManager.GetSceneByName("Infiltration").GetRootGameObjects();

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        fpsController = GetComponent<FirstPersonController>();
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
        if( fpsController.m_isActive )
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, raycastRange);
            if ( isHit && Input.GetKeyDown(KeyCode.Mouse0) && hit.transform.name == "Cam1Feed" && securityCamControllers[0] )
            {
                Cursor.lockState = CursorLockMode.Locked;
                fpsController.m_isActive = false;
                securityCamControllers[0].isCamActive = true;
                return;
            }
        }
    }
}
