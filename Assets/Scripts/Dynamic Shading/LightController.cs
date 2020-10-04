using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movementDirection = Vector3.zero;
        luxControlsActivated = false;
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
        luxModel.LookAt(luxModel.position + movementDirection);
        anim.SetBool("Moving", true);
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
