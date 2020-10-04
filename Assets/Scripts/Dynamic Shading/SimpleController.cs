using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    public float moveSpeed = 10;
    public float acceleration = 20;
    public float jumpForce;
    public float jumpFalloff = 0f;
    public float jumpFalloffMultiplier = 2.5f;
    public float jumpFallMultiplier = 3.5f;
    public float raycastLength = 3f;
    public Rigidbody2D rb2D;
    Collider2D collider2D;
    MeshRenderer meshRenderer;

    bool isGrounded;
    bool canJump;
    float lastFrameYPos;

    //int frameCount;

    float speed;
    bool isMoving;
    Vector3 direction;

    //3D/2D state
    bool isIn2D = true;
    public LayerMask wall2DLayermask;
    public LayerMask wall3DLayerMask;
    public LayerMask obstacleLayerMask;

    //Original position/rotation
    Vector3 originalPosition;
    Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        meshRenderer = GetComponent<MeshRenderer>();
        isMoving = false;
        direction = Vector3.zero;

        //frameCount = 0;

        isIn2D = true; // For sake of Theatre_v2 demo

        //Save original position/rotation
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        isGrounded = true;
        canJump = true;
    }

    private void FixedUpdate()
    {     
        transform.position += direction * speed * Time.deltaTime;

        //jump
        if(lastFrameYPos == transform.position.y)
        {
            canJump = true;
        }
        lastFrameYPos = transform.position.y;

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb2D.AddForce(Vector3.up * jumpForce);
            transform.position += new Vector3(0, 0.1f, 0);
            isGrounded = false;
            canJump = false;
            //frameCount = 2;
        }

        if (rb2D.velocity.y < jumpFalloff)
            rb2D.gravityScale = jumpFalloffMultiplier;
        else if (rb2D.velocity.y < 0)
            rb2D.gravityScale = jumpFallMultiplier;
        else
            rb2D.gravityScale = 1;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();

        if(isIn2D)
        {
            //Check if player is squished
            bool isSquished = CheckIfSquished();
            if(isSquished)
            {
                ResetPlayer();
            }
        }

    }

    void UpdateSpeed()
    {
        if (isMoving)
        {
            if (speed < moveSpeed)
            {
                speed += acceleration * Time.deltaTime;
            }
            else
            {
                speed = moveSpeed;
            }
        }
        else
        {
            if (speed > 0)
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
            rb2D.velocity = Vector2.right * 0.0f + Vector2.up * rb2D.velocity.y;

            if (direction == Vector3.right)
            {
                speed = 0;
            }
            direction = Vector3.left;
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb2D.velocity = Vector2.right * 0.0f + Vector2.up * rb2D.velocity.y;
            //transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            if (direction == Vector3.left)
            {
                speed = 0;
            }
            direction = Vector3.right;
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //Inputs for switching inbetween 2D and 3D
        if(Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Left click pressed");
            //SwitchRealm();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.GetContact(0);
        //frameCount -= 1;
        if (contactPoint.normal.y >= 0.5f)// && frameCount <= 0
        {
            isGrounded = true;
            //frameCount = 0;
        }
        //rb.velocity = Vector2.right * 0.0f + Vector2.up;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ContactPoint2D contactPoint = collision.GetContact(0);
        if (contactPoint.normal.y >= 0.5f)
            canJump = true;
        //rb.velocity = Vector2.right * 0.0f + Vector2.up;
        //canJump = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        rb2D.velocity = Vector2.right * 0.0f + Vector2.up * rb2D.velocity.y;
        /*ContactPoint2D contactPoint = collision.GetContact(0);
        if (contactPoint.normal.y >= 0.5f)
        {
            //isGrounded = false;
        }*/
    }

    public void SwitchRealm()
    {
        // Check if in 2D space
        if(isIn2D)
        {
            // Shoot raycast to player's left and right to determine which side the wall is
            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position, transform.forward, out hitInfo, Mathf.Infinity, wall2DLayermask, QueryTriggerInteraction.Ignore))
            {
                // Get the Wall2D component of collided object
                Wall2D wall2D = hitInfo.collider.gameObject.GetComponent<Wall2D>();
                if(wall2D)
                {
                    // Move player to the corresponding 3D wall
                    transform.position = wall2D.SwitchTo3D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point));

                    //collider2D.enabled = false;
                    //rb2D.gravityScale = 0;
                    rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
                    //meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
            else if (Physics.Raycast(transform.position, -transform.forward, out hitInfo, Mathf.Infinity, wall2DLayermask, QueryTriggerInteraction.Ignore))
            {
                // Get the Wall2D component of collided object
                Wall2D wall2D = hitInfo.collider.gameObject.GetComponent<Wall2D>();
                if (wall2D)
                {
                    // Move player to the corresponding 3D wall
                    transform.position = wall2D.SwitchTo3D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point));

                    //collider2D.enabled = false;
                    //rb2D.gravityScale = 0;
                    rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
                    //meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }

            isIn2D = false; // State has now changed to 3D
        }
        else //TODO: third person controls to determine the correct orientation for going back to 2D
        {
            RaycastHit hitInfo;
            if(Physics.Raycast(transform.position, Vector3.forward, out hitInfo, 5.0f, wall3DLayerMask, QueryTriggerInteraction.Collide))
            {
                // Get the Wall3D component of collided object
                Wall3D wall3D = hitInfo.collider.gameObject.GetComponent<Wall3D>();
                if(wall3D)
                {
                    // Move player to the corresponding 2D wall
                    transform.position = wall3D.SwitchTo2D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point));

                    //collider2D.enabled = true;
                    //rb2D.gravityScale = 1;
                    rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                    //meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }

            isIn2D = true; // State has now changed to 2D
        }
    }

    bool CheckIfSquished()
    {
        Debug.DrawRay(transform.position, Vector2.right * raycastLength, Color.green);
        Debug.DrawRay(transform.position, Vector2.left * raycastLength, Color.green);
        bool ret = false;
        //Debug.Log("checking if squished");
        if(Physics2D.Raycast(transform.position, Vector2.right, raycastLength, obstacleLayerMask)
            && Physics2D.Raycast(transform.position, Vector2.left, raycastLength, obstacleLayerMask))
        {
            //Debug.Log("squished");
            ret = true;
        }
        return ret;
    }

    void ResetPlayer()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb2D.velocity = Vector2.zero;
    }
    
}
