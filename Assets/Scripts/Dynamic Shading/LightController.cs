﻿using System.Collections;
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
    public static bool luxControlsActivated;
    public static bool activate = false;

    private bool isClimb = false;
    private bool isTouch = false;
    // integer to denote the direction
    // 0 no move, 1 up, 2 down
    private int climbDir = 0;

    // bool to control whether Lux can move in z direction
    private bool enableForwardBack;

    [SerializeField] private GameObject runningTransform;
    public GameObject currLadder;
    public Vector3 currLadder_collider_center;

    public CameraSwitch cameraSwitch;

    private Vector3 forwardDir;
    private Vector3 rightDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        forwardDir = Vector3.forward;
        rightDir = Vector3.Cross(Vector3.up, forwardDir);
    }

    private void OnEnable()
    {
        activate = true;
        enableForwardBack = true;
    }

    void Update()
    {
        movementDirection = Vector3.zero;
        luxControlsActivated = false;

        if(isTouch && climbDir != 0 && Input.GetAxis("Interaction") > 0.8f)
        {

            currLadder_collider_center = currLadder.transform.position + currLadder.GetComponent<BoxCollider>().center;
            Vector3 curr_position = transform.position;
            curr_position.x = currLadder_collider_center.x;
            curr_position.z = currLadder_collider_center.z;
            transform.position = curr_position;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            runningTransform.transform.rotation = Quaternion.Euler(0, 0, 0);

            isClimb = true;
            PlayAnim();

            if (cameraSwitch != null && cameraSwitch.gameObject.activeInHierarchy)
            {
                if (climbDir == 1)
                {
                    cameraSwitch.ChangeToCamera(1);
                }
                else if (climbDir == 2)
                {
                    cameraSwitch.ChangeToCamera(0);
                }
            }
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
            movementDirection -= rightDir;
            luxControlsActivated = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementDirection += rightDir;
            luxControlsActivated = true;
        }

        if (enableForwardBack)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                movementDirection += forwardDir;
                luxControlsActivated = true;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                movementDirection -= forwardDir;
                luxControlsActivated = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if(movementDirection == Vector3.zero)
        {
            rb.velocity = new Vector3(0f, -1f, 0f);
            if (!luxControlsActivated)
               anim.SetBool("Moving", false);
            return;
        }

        rb.velocity = movementDirection.normalized * moveSpeed;
        if (movementDirection.y == 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, -1f, rb.velocity.z);
        }

        // Make sure Lux does not look upward or downward
        Vector3 sightDirection = movementDirection;
        sightDirection.y = 0f;
        if (sightDirection != Vector3.zero)
        {
            luxModel.LookAt(luxModel.position + sightDirection);
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

                anim.SetBool("TouchUp", false);
                climbDir = 0;
                isClimb = false;
            }
            else if(!isClimb)
            {
                // anim.SetBool("TouchDown", false);
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
                // curr_position.y -= 1f;
                curr_position.z = currLadder_collider_center.z - 0.25f;
                transform.position = curr_position;

                anim.SetBool("TouchDown", false);
                climbDir = 0;
                isClimb = false;
            }
            else if (!isClimb)
            {
                // anim.SetBool("TouchUp", false);
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
            anim.SetBool("TouchUp", true);
        }
        else
        {
            anim.SetBool("TouchDown", true);
        }
    }

    public void SetEnableForwardBack(bool val)
    {
        enableForwardBack = val;
    }

    public void SetForwardDir(Vector3 dir)
    {
        forwardDir = dir;
        rightDir = Vector3.Cross(Vector3.up, forwardDir);
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
