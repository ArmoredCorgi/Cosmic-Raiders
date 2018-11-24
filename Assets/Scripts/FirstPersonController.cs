using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour {

    public LayerMask groundLayers;
    public float walkingSpeed = 2.0f;
    public float runningSpeed = 4.0f;
    public float jumpHeight = 1f;

    private CapsuleCollider col;
    private Rigidbody rb;
    private float speed = 2.0f;

    // Use this for initialization
    void Start ()
    {
        speed = walkingSpeed;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( Input.GetKeyDown(KeyCode.Escape) ) {  Cursor.lockState = CursorLockMode.None;  }
        if( Input.GetKeyDown(KeyCode.Mouse0) ) {  Cursor.lockState = CursorLockMode.Locked;  }
        if( Input.GetKey(KeyCode.LeftShift) ) {  speed = runningSpeed;  }
        if( Input.GetKeyUp(KeyCode.LeftShift) ) {  speed = walkingSpeed;  }
        
        float forward = Input.GetAxis("Vertical");
        float strafe = Input.GetAxis("Horizontal");

        forward *= Time.deltaTime * speed;
        strafe *= Time.deltaTime * speed;
        
        transform.Translate(strafe, 0, forward);

        if( IsGrounded() && Input.GetKeyDown(KeyCode.Space) )
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
        
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * 0.1f, groundLayers);
    }
}
