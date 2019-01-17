using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class RaycastingController : MonoBehaviour {
    
    public bool isPlayerActive;

    [SerializeField] List<SecurityCamController> securityCamControllers = new List<SecurityCamController>();
    [SerializeField] readonly float raycastRange = 2.0f;

    GameObject[] SecurityCams;
    CapsuleCollider col;
    Rigidbody rb;
    Camera fpsCam;
    float speed = 2.0f;
    private FirstPersonController fpsController;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        fpsController = GetComponent<FirstPersonController>();
        fpsCam = GetComponentInChildren<Camera>();

        //----THE FOLLOWING WAS MOVED TO RAIDERS HUB MANAGER.CS - IF THAT IS CAUSING PROBLEMS RESTORE THIS, OTHERWISE DELETE:

        //if (!SceneManager.GetSceneByBuildIndex(1).isLoaded)
        //{
        //    SceneManager.LoadScene(1, LoadSceneMode.Additive);
        //}

        //SceneManager.MergeScenes(SceneManager.GetSceneByBuildIndex(0), SceneManager.GetSceneByBuildIndex(1));

        //----

        SecurityCams = GameObject.FindGameObjectsWithTag("SecurityCam");

        foreach( GameObject cam in SecurityCams )
        {
            securityCamControllers.Add(cam.GetComponent<SecurityCamController>());
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        if( fpsController.m_isActive )
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, raycastRange);
            if ( isHit 
                && Input.GetKeyDown(KeyCode.Mouse0) 
                && hit.transform.name == "Cam1Feed" 
                && securityCamControllers[0] )
            {
                Cursor.lockState = CursorLockMode.Locked;
                fpsController.m_isActive = false;
                securityCamControllers[0].isCamActive = true;
                return;
            }
        }
    }
}
