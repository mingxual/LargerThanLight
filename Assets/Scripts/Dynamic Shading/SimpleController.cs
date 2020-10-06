using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour
{
    [SerializeField] private Transform skiaModel;
    [SerializeField] private Animator anim;

    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float jumpSpeed = 10;
    [SerializeField] private float fallMultiplier = 3.5f;
    [SerializeField] private float defaultMultiplier = 2f;
    [SerializeField] private float rayboxDistance = 0.05f;
    [SerializeField] private LayerMask mask;

    private Rigidbody2D rb;
    private float movementDirection;
    private bool skiaControlsActivated;
    private bool jumpAction;
    private bool grounded;
    private Vector2 playerCenter;
    private Vector2 playerSize;
    private Vector2 rayboxSize;
    private float squishDistance;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private Vector3 leftFacingDirection;
    private Vector3 rightFacingDirection;

    //3D/2D state
    bool isIn2D = true;
    public LayerMask wall2DLayermask;
    public LayerMask wall3DLayerMask;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerCenter = GetComponent<BoxCollider2D>().offset;
        playerSize = GetComponent<BoxCollider2D>().size;
        rayboxSize = new Vector2(playerSize.x - rayboxDistance, rayboxDistance);
        squishDistance = playerSize.x * 0.5f;

        leftFacingDirection = new Vector3(skiaModel.localScale.x, skiaModel.localScale.y, -skiaModel.localScale.z);
        rightFacingDirection = new Vector3(skiaModel.localScale.x, skiaModel.localScale.y, skiaModel.localScale.z);
    }

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        //mainCamera = Camera.main;
    }

    private void Update()
    {
        movementDirection = 0;
        skiaControlsActivated = false;
        if (Input.GetKey(KeyCode.A))
        {
            movementDirection -= 1;
            skiaControlsActivated = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementDirection += 1;
            skiaControlsActivated = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && grounded)
        {
            jumpAction = true;
        }

        //Inputs for switching inbetween 2D and 3D
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Left click pressed");
            //SwitchRealm();
        }

        if (isIn2D)
        {
            //Check if player is squished
            bool isSquished = CheckIfSquished();
            if (isSquished)
            {
                ResetPlayer();
            }
        }

        //CheckObjectBlocking();
    }

    private void FixedUpdate()
    {
        if (jumpAction)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpAction = false;
            grounded = false;
            anim.Play("Jump");
        }
        else
        {
            Vector2 rayboxCenter = (Vector2)transform.position + playerCenter + Vector2.down * (playerSize.y + rayboxSize.y) * 0.5f;
            grounded = (Physics2D.OverlapBox(rayboxCenter, rayboxSize, 0, mask) != null);
        }

        rb.velocity = new Vector2(movementDirection * moveSpeed, rb.velocity.y);
        if(movementDirection == 0)
        {
            if (!skiaControlsActivated)
                anim.SetBool("IsRunning", false);
        }
        else
        {
            skiaModel.localScale = movementDirection > 0 ? rightFacingDirection : leftFacingDirection;
            anim.SetBool("IsRunning", true);
        }

        if (rb.velocity.y < 0)
            rb.gravityScale = fallMultiplier;
        else
            rb.gravityScale = defaultMultiplier;
    }

    void ResetPlayer()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        rb.velocity = Vector2.zero;
    }

    public void SetNewCheckpoint()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        rb.velocity = Vector2.zero;
    }

    //private Camera mainCamera;
    //private List<Renderer> obstacleRendererCache = new List<Renderer>();

    //private void CheckObjectBlocking()
    //{
    //    RaycastHit wall2dhit;
    //    if(Physics.Raycast(transform.position, transform.forward, out wall2dhit, 5, wall2DLayermask))
    //    {
    //        Wall2D wall2d = wall2dhit.collider.GetComponent<Wall2D>();
    //        print(wall2d);
    //        if(wall2d != null)
    //        {
    //            print("yeey");
    //            Vector3 wall3dpos = wall2d.SwitchTo3D(wall2dhit.transform.InverseTransformPoint(wall2dhit.point));
    //            RaycastHit[] hits = Physics.SphereCastAll(wall3dpos, 4, mainCamera.transform.position - wall3dpos, 20);
    //            foreach(RaycastHit hit in hits)
    //            {
    //                print("boom");
    //                if (hit.transform.tag == "Obstacle")
    //                {
    //                    Renderer obsRenderer = hit.transform.GetComponent<Renderer>();
    //                    if(!obstacleRendererCache.Contains(obsRenderer))
    //                    {
    //                        Color c = obsRenderer.material.color;
    //                        c.a = 0.5f;
    //                        obsRenderer.material.color = c;

    //                        obstacleRendererCache.Add(obsRenderer);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public void SwitchRealm()
    {
        // Check if in 2D space
        if (isIn2D)
        {
            // Shoot raycast to player's left and right to determine which side the wall is
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, Mathf.Infinity, wall2DLayermask, QueryTriggerInteraction.Ignore))
            {
                // Get the Wall2D component of collided object
                Wall2D wall2D = hitInfo.collider.gameObject.GetComponent<Wall2D>();
                if (wall2D)
                {
                    // Move player to the corresponding 3D wall
                    transform.position = wall2D.SwitchTo3D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point));

                    //collider2D.enabled = false;
                    //rb2D.gravityScale = 0;
                    rb.constraints = RigidbodyConstraints2D.FreezePosition;
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
                    rb.constraints = RigidbodyConstraints2D.FreezePosition;
                    //meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }

            isIn2D = false; // State has now changed to 3D
        }
        else //TODO: third person controls to determine the correct orientation for going back to 2D
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, Vector3.forward, out hitInfo, 5.0f, wall3DLayerMask, QueryTriggerInteraction.Collide))
            {
                // Get the Wall3D component of collided object
                Wall3D wall3D = hitInfo.collider.gameObject.GetComponent<Wall3D>();
                if (wall3D)
                {
                    // Move player to the corresponding 2D wall
                    transform.position = wall3D.SwitchTo2D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point));

                    //collider2D.enabled = true;
                    //rb2D.gravityScale = 1;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    //meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }

            isIn2D = true; // State has now changed to 2D
        }
    }

    bool CheckIfSquished()
    {
        //Debug.DrawRay(transform.position, Vector2.right * raycastLength, Color.green);
        //Debug.DrawRay(transform.position, Vector2.left * raycastLength, Color.green);
        bool ret = false;
        //Debug.Log("checking if squished");
        if (Physics2D.Raycast(transform.position, Vector2.right, squishDistance, mask)
            && Physics2D.Raycast(transform.position, Vector2.left, squishDistance, mask))
        {
            //Debug.Log("squished");
            ret = true;
        }
        return ret;
    }
}
