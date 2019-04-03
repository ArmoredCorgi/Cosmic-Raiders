using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMinigameController : MonoBehaviour
{
    //TODO: Randomly generated (winnable) mazes

    [SerializeField] private RectTransform goalPos;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject reticle;
    [SerializeField] private GameObject hackingMenu;
    [SerializeField] private RaycastingController raycastingController;

    private Vector3 initPos;
    private float dx, dy;
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        canMove = false;
        initPos = rectTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetKey(KeyCode.Mouse0) )
        {
            Vector2 clickPos = Input.mousePosition;

             if( Mathf.Abs(rectTransform.position.x - clickPos.x) <= 10f &&
                 Mathf.Abs(rectTransform.position.y - clickPos.y) <= 10f)
             {
                rectTransform.position = new Vector2(clickPos.x, clickPos.y);
             }
        }

        if (Mathf.Abs(rectTransform.position.x - goalPos.position.x) <= 5f &&
                Mathf.Abs(rectTransform.position.y - goalPos.position.y) <= 5f)
        {
            PuzzleDone(success: true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("TRIGGERED WITH: " + other.gameObject);
        if (other.gameObject.tag == Tags.mazeWall)
            PuzzleDone(success: false);
    }

    private void PuzzleDone(bool success)
    {
        if (success) {
            var doorAnim = door.GetComponent<Animator>();
            doorAnim.SetBool("OpenDoor", true);
            raycastingController.CloseMenu(reticle: reticle, hackingMenu: hackingMenu);

            rectTransform.position = initPos; //reset maze
        }
        else
        {
            rectTransform.position = initPos; //reset maze
        }
    }
}
