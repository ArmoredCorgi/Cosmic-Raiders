using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class RaycastingController : MonoBehaviour {
    
    public bool isInHackingMenu;
    public Canvas pcCanvas;

    [SerializeField] List<SecurityCamController> securityCamControllers = new List<SecurityCamController>();
    [SerializeField] readonly float raycastRange = 2.0f;

    GameObject[] SecurityCams;
    Camera fpsCam;
    private FirstPersonController fpsController;

    // Use this for initialization
    void Start ()
    {
        isInHackingMenu = false;

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
        //TODO: Migrate to PCUIController
        var reticle = pcCanvas.transform.Find("Image_Reticle").gameObject;
        var hackingMenu = pcCanvas.transform.Find("HackingMenu").gameObject;

        if ( fpsController.enabled && Input.GetKeyDown(KeyCode.Mouse0) )
        {
            RaycastHit hit;
            bool isHit = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, raycastRange);

            print(hit.transform.tag);

            if ( isHit  
                && hit.transform.tag == Tags.camFeed 
                && securityCamControllers.Count > 0 )
            {
                print("CAMFEED HIT, CAMS EXIST");

                Cursor.lockState = CursorLockMode.Locked;

                string feedName = hit.transform.name;
                char feedNumChar = feedName[feedName.Length - 1];

                int feedNum = (int)char.GetNumericValue(feedNumChar);

                if( feedNum <= securityCamControllers.Count )
                {
                    fpsController.enabled = false;
                    securityCamControllers[feedNum - 1].isCamActive = true;
                }

                return;
            }
            if ( isHit
                && hit.transform.tag == Tags.hackingTerminal)
            {
                print("TERMINAL HIT");

                fpsController.enabled = false;
                isInHackingMenu = true;
            }
        }

        if( isInHackingMenu && pcCanvas != null )
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Cursor.visible = false;
                hackingMenu.SetActive(false);
                reticle.SetActive(true);
                fpsController.enabled = true;
                isInHackingMenu = false;
                return;
            }

            if (reticle != null)
                reticle.SetActive(false);
            if (hackingMenu != null)
                hackingMenu.SetActive(true);
        }
    }
}
