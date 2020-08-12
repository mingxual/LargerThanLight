using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    Rigidbody2D rb;
    bool canJump;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = Vector2.right * 0.0f + Vector2.up * rb.velocity.y;
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = Vector2.right * 0.0f + Vector2.up * rb.velocity.y;
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.W) && canJump)
        {
            rb.AddForce(Vector3.up * jumpForce);
            canJump = false;
        }
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
