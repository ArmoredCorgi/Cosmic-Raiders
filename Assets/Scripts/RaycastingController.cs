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
    Camera fpsCam;
    private FirstPersonController fpsController;

    // Use this for initialization
    void Start ()
    {
        fpsController = GetComponent<FirstPersonController>();
        fpsCam = GetComponentInChildren<Camera>();

        //Delay added to allow for scene loading prior to set up:
        Invoke("SecurityCamSetup", 0.5f);
	}

    private void SecurityCamSetup()
    {
        SecurityCams = GameObject.FindGameObjectsWithTag("SecurityCam");

        foreach (GameObject cam in SecurityCams)
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
                && hit.transform.tag == "CamFeed" 
                && securityCamControllers.Count > 0 )
            {
                print("CAMFEED HIT, CAMS EXIST");

                Cursor.lockState = CursorLockMode.Locked;

                string feedName = hit.transform.name;
                char feedNumChar = feedName[feedName.Length - 1];

                int feedNum = (int)char.GetNumericValue(feedNumChar);

                if( feedNum <= securityCamControllers.Count )
                {
                    fpsController.m_isActive = false;
                    securityCamControllers[feedNum - 1].isCamActive = true;
                }

                return;
            }
        }
    }
}
