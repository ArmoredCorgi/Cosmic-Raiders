using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class SecurityCamController : MonoBehaviour {

    public GameObject hJoint;
    public GameObject vJoint;
    public float sensitivity = 2.0f;
    public float smoothing = 2.0f;
    public bool isCamActive;
    
    Vector2 mouseLook;
    Vector2 smoothV;
    GameObject[] RaidersHubGOArray;
    FirstPersonController fpsController;

	// Use this for initialization
	void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isCamActive = false;

        //Delay added to allow for scene loading prior to set up:
        Invoke("PlayerTagGOSetup", 0.5f);
    }

    private void PlayerTagGOSetup ()
    {
        RaidersHubGOArray = GameObject.FindGameObjectsWithTag("Player");

        foreach ( GameObject go in RaidersHubGOArray )
        {
            fpsController = go.GetComponent<FirstPersonController>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if( isCamActive )
        {
            var mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            mouse = Vector2.Scale(mouse, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, mouse.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, mouse.y, 1f / smoothing);
            mouseLook += smoothV;
            mouseLook.x = Mathf.Clamp(mouseLook.x, -45f, 45f);
            mouseLook.y = Mathf.Clamp(mouseLook.y, -45f, 45f);

            hJoint.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
            vJoint.transform.localRotation = Quaternion.Euler(-mouseLook.y, mouseLook.x - 90, 0);

            if( Input.GetKeyDown(KeyCode.Mouse1) )
            {
                isCamActive = false;
                fpsController.m_isActive = true;
            }
        }
    }
}
