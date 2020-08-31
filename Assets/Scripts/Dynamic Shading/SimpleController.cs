using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    public float moveSpeed;
    public float acceleration;
    public float jumpForce;
    Rigidbody2D rb;
    bool canJump;

    float speed;
    bool isMoving;
    Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isMoving = false;
        direction = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            if(speed < 10)
            {
                speed += acceleration * Time.deltaTime;
            }
            else
            {
                speed = 10;
            }
        }
        else
        {
            if(speed > 0)
            {
                speed -= acceleration * Time.deltaTime;
            }
            else
            {
                speed = 0;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            //rb.velocity = Vector2.right * 0.0f + Vector2.up * rb.velocity.y;
            direction = Vector3.left;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //rb.velocity = Vector2.right * 0.0f + Vector2.up * rb.velocity.y;
            //transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            direction = Vector3.right;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce);
            canJump = false;
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.GetContact(0);
        if (contactPoint.normal.y >= 0.0f)
            canJump = true;
        //rb.velocity = Vector2.right * 0.0f + Vector2.up;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        /*ContactPoint2D contactPoint = collision.GetContact(0);
        if (contactPoint.normal.y >= 0.0f)
            canJump = true;*/
        //rb.velocity = Vector2.right * 0.0f + Vector2.up;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rb.velocity = Vector2.right * 0.0f + Vector2.up * rb.velocity.y;
    }

    
}
