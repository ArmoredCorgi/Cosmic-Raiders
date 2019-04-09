using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class RaycastingController : MonoBehaviour {
    
    public bool isInHackingMenu;
    public int currentScreenNum;
    public Canvas pcCanvas;

    [SerializeField] List<SecurityCamController> securityCamControllers = new List<SecurityCamController>();
    [SerializeField] readonly float raycastRange = 2.0f;

    GameObject[] securityCams;
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
        securityCams = GameObject.FindGameObjectsWithTag("SecurityCam");

        foreach (GameObject cam in securityCams)
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

            //Change large screen's feed to selected screen's feed:
            if( isHit
                && hit.transform.tag == Tags.screen
                && securityCamControllers.Count > 0)
            {
                string screenName = hit.transform.name;
                char screenNumChar = screenName[screenName.Length - 1];
                int screenNum = (int)char.GetNumericValue(screenNumChar);

                if (screenNum <= securityCams.Length)
                {
                    var securityCam = securityCams[screenNum - 1];
                    var cam = securityCam.GetComponentInChildren<Camera>();
                    var texture = new RenderTexture(1024, 1024, 16);
                    Material material = new Material(Shader.Find("Standard"));

                    cam.targetTexture = texture;
                    material.mainTexture = texture;
                    
                    var screen = GameObject.Find("Screen" + screenNum);
                    Renderer screenRenderer = screen.GetComponent<Renderer>();
                    screenRenderer.material = material;

                    var screenL = GameObject.Find("ScreenL");
                    Renderer screenRendererL = screenL.GetComponent<Renderer>();
                    screenRendererL.material = material;

                    currentScreenNum = screenNum;
                }
            }

            //Move large screen's camera:
            if ( isHit  
                && hit.transform.tag == Tags.camFeed 
                && securityCamControllers.Count > 0 )
            {
                print("CAMFEED HIT, CAMS EXIST");

                Cursor.lockState = CursorLockMode.Locked;

                if( currentScreenNum <= securityCamControllers.Count )
                {
                    fpsController.enabled = false;
                    securityCamControllers[currentScreenNum - 1].isCamActive = true;
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
                CloseMenu(reticle: reticle, hackingMenu: hackingMenu);
                return;
            }

            if (reticle != null)
                reticle.SetActive(false);
            if (hackingMenu != null)
                hackingMenu.SetActive(true);
        }
    }

    public void CloseMenu(GameObject reticle, GameObject hackingMenu)
    {
        Cursor.visible = false;
        hackingMenu.SetActive(false);
        reticle.SetActive(true);
        fpsController.enabled = true;
        isInHackingMenu = false;
    }
}
