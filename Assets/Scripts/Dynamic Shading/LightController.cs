using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LightController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    //[SerializeField] private float rotateSpeed;

    [SerializeField] private Animator anim;
    [SerializeField] private Transform luxModel;
    //public GameObject EmptyGO;

    private Rigidbody rb;
    private Vector3 movementDirection;
    private bool luxControlsActivated;

    private bool isClimb = false;
    private bool isTouch = false;
    // integer to denote the direction
    // 0 no move, 1 up, 2 down
    private int climbDir = 0;

    [SerializeField] private GameObject runningTransform;
    public GameObject currLadder;
    public Vector3 currLadder_collider_center;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movementDirection = Vector3.zero;
        luxControlsActivated = false;

        if(isTouch && climbDir != 0 && Input.GetKeyDown(KeyCode.C))
        {

            currLadder_collider_center = currLadder.transform.position + currLadder.GetComponent<BoxCollider>().center;
            Vector3 curr_position = transform.position;
            curr_position.x = currLadder_collider_center.x;
            curr_position.z = currLadder_collider_center.z;
            transform.position = curr_position;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            runningTransform.transform.rotation = Quaternion.Euler(0, 0, 0);

            isClimb = true;
            StartCoroutine("PlayAnim");
        }

        if(isClimb)
        {
            if (climbDir == 1)
            {
                movementDirection.y = 1;
            }
            else
            {
                movementDirection.y = -1;
            }
            luxControlsActivated = true;
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementDirection.x -= 1;
            luxControlsActivated = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementDirection.x += 1;
            luxControlsActivated = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementDirection.z += 1;
            luxControlsActivated = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movementDirection.z -= 1;
            luxControlsActivated = true;
        }
    }

    private void FixedUpdate()
    {
        if(movementDirection == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            if(!luxControlsActivated)
               anim.SetBool("Moving", false);
            return;
        }

        rb.velocity = movementDirection.normalized * moveSpeed;
        // Make sure Lux does not look upward or downward
        movementDirection.y = 0;
        if (movementDirection != Vector3.zero)
        {
            luxModel.LookAt(luxModel.position + movementDirection);
            anim.SetBool("Moving", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TouchUp")
        {
            currLadder = other.gameObject;
            if (isClimb && climbDir == 1)
            {
                // Set the position
                currLadder_collider_center = currLadder.transform.position + currLadder.GetComponent<BoxCollider>().center;
                Vector3 curr_position = transform.position;
                curr_position.x = currLadder_collider_center.x;
                // Move in a little bit to land on the platform
                curr_position.z = currLadder_collider_center.z + 1.5f;
                transform.position = curr_position;

                anim.SetBool("TouchUp", true);
                climbDir = 0;
                isClimb = false;
            }
            else if(!isClimb)
            {
                anim.SetBool("TouchDown", false);
                climbDir = 2;
            }
        }
        else if(other.tag == "TouchDown")
        {
            currLadder = other.gameObject;
            if (isClimb && climbDir == 2)
            {
                // Set the position
                currLadder_collider_center = currLadder.transform.position + currLadder.GetComponent<BoxCollider>().center;
                Vector3 curr_position = transform.position;
                curr_position.x = currLadder_collider_center.x;
                // Move in a little bit to land on the platform
                curr_position.y -= 1f;
                curr_position.z = currLadder_collider_center.z - 0.25f;
                transform.position = curr_position;

                anim.SetBool("TouchDown", true);
                climbDir = 0;
                isClimb = false;
            }
            else if (!isClimb)
            {
                anim.SetBool("TouchUp", false);
                climbDir = 1;
            }
        }

        isTouch = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "TouchUp" || other.tag == "TouchDown")
        {
            isTouch = false;
        }
    }

    void PlayAnim()
    {
        if(climbDir == 1)
        {
            //anim.Play("ClimbUp");
        }
        else
        {
            //anim.Play("ClimbDown");
        }
    }

    //void Update()
    //{
    //    if (Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    //        anim.SetBool("Moving", true);
    //    }
    //    if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    //        anim.SetBool("Moving", true);

    //    }
    //    if (Input.GetKey(KeyCode.UpArrow))
    //    {
    //        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
    //        anim.SetBool("Moving", true);

    //    }
    //    if (Input.GetKey(KeyCode.DownArrow))
    //    {
    //        transform.position += Vector3.back * moveSpeed * Time.deltaTime;
    //        anim.SetBool("Moving", true);

    //    }

    //    //he stops running animation when no key is pressed
    //    if (Input.anyKey == false)
    //    {
    //        anim.SetBool("Moving", false);
    //    }

    //    //I just hardcoded lux's size for testing purposes
    //    /* if (Input.GetKeyDown(KeyCode.LeftArrow))
    //     {
    //         luxModel.transform.localScale = new Vector3(14.3759f, 14.3759f, -14.3759f);
    //     }
    //     if (Input.GetKeyDown(KeyCode.RightArrow))
    //     {
    //         luxModel.transform.localScale = new Vector3(14.3759f, 14.3759f, 14.3759f);
    //     }*/

    //    float moveVertical = Input.GetAxis("Vertical");
    //    float moveHorizontal = Input.GetAxis("Horizontal");

    //    Vector3 newPosition = new Vector3(moveHorizontal, 0.0f, moveVertical);
    //    luxModel.transform.LookAt(newPosition + luxModel.transform.position);
    //    //luxModel.transform.Translate(newPosition * Time.deltaTime, Space.World);

    //    /*if (Input.GetKey(KeyCode.L))
    //    {
    //        transform.Rotate(0.0f, rotateSpeed * Time.deltaTime, 0.0f);
    //    }
    //    if (Input.GetKey(KeyCode.J))
    //    {
    //        transform.Rotate(0.0f, -rotateSpeed * Time.deltaTime, 0.0f);
    //    }
    //    if (Input.GetKey(KeyCode.I))
    //    {
    //        transform.Rotate(-rotateSpeed * Time.deltaTime, 0.0f, 0.0f);
    //    }
    //    if (Input.GetKey(KeyCode.K))
    //    {
    //        transform.Rotate(rotateSpeed * Time.deltaTime, 0.0f, 0.0f);
    //    }*/
    //}
}
