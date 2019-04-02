using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMinigameController : MonoBehaviour
{
    [SerializeField] private RectTransform goalPos;
    [SerializeField] private RectTransform rectTransform;
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
            print("GetKeyDown");

             if( Mathf.Abs(rectTransform.position.x - clickPos.x) <= 10f &&
                 Mathf.Abs(rectTransform.position.y - clickPos.y) <= 10f)
             {
                print("Began touching player node");
                rectTransform.position = new Vector2(clickPos.x, clickPos.y);
             }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (Mathf.Abs(rectTransform.position.x - goalPos.position.x) <= 5f &&
                Mathf.Abs(rectTransform.position.y - goalPos.position.y) <= 5f) //Play with these to see if the distances from goal are accurate
            {
                PuzzleDone(success: true);
            }
            else
            {
                PuzzleDone(success: false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.tag == Tags.mazeWall )
        {
            PuzzleDone(success: false);
        }
    }

    private void PuzzleDone(bool success)
    {
        if (success) {
            print("SUCCESS");
        }
        else
        {
            print("FAILURE");
            rectTransform.position = initPos; //reset maze
        }
    }
}
