using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamController : MonoBehaviour {

    public GameObject hJoint;
    public GameObject vJoint;
    public float sensitivity = 2.0f;
    public float smoothing = 2.0f;
    
    Vector2 mouseLook;
    Vector2 smoothV;

	// Use this for initialization
	void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update ()
    {
        var mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouse = Vector2.Scale(mouse, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, mouse.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouse.y, 1f / smoothing);
        mouseLook += smoothV;
        mouseLook.x = Mathf.Clamp(mouseLook.x, 0f, 90f);
        mouseLook.y = Mathf.Clamp(mouseLook.y, -45f, 45f);

        hJoint.transform.rotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
        vJoint.transform.rotation = Quaternion.Euler(-mouseLook.y, mouseLook.x - 90, 0);

        if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if ( Input.GetKeyDown(KeyCode.Mouse0) )
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
