using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour {
    
    public float bobWalkingSpeed = 0.12f;
    public float bobRunningSpeed = 0.24f;
    public float bobbingAmount = 0.04f;

    FirstPersonController fpsController;
    private float bobbingSpeed = 0.12f;
    private float midPoint;
    private float timer = 0.0f;

    void Start()
    {
        midPoint = transform.localPosition.y;
        bobbingSpeed = bobWalkingSpeed;

        fpsController = GetComponent<FirstPersonController>();
    }

    void Update ()
    {
        if( fpsController.isPlayerActive )
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                bobbingSpeed = bobRunningSpeed;
            }
            else
            {
                bobbingSpeed = bobWalkingSpeed;
            }

            var waveSlice = 0.0f;
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            if (Mathf.Abs(horizontal) == 0 & Mathf.Abs(vertical) == 0)
            {
                timer = 0.0f;
            }
            else
            {
                waveSlice = Mathf.Sin(timer);
                timer = timer + bobbingSpeed;
                if (timer > Mathf.PI * 2)
                {
                    timer = timer - (Mathf.PI * 2);
                }
            }
            if (waveSlice != 0)
            {
                var translateChange = waveSlice * bobbingAmount;
                var totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                translateChange = totalAxes * translateChange;
                transform.localPosition = new Vector3(transform.localPosition.x, midPoint + translateChange, transform.localPosition.z);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, midPoint, transform.localPosition.z);
            }
        }
        
	}
}
