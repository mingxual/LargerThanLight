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

    LevelManager m_LevelManager;

    private Rigidbody2D rb;    
    private float movementDirection;
    private bool skiaControlsActivated;
    private bool jumpAction;
    private bool grounded;
    private Vector2 playerCenter;
    private Vector2 playerSize;
    private Vector2 rayboxSize;
    private float squishDistance;

    private BoxCollider2D collider;
    private Vector2 colliderCenter;
    private Vector2 colliderSize;
    bool jumping;

    public Vector3 originalPosition;
    private Quaternion originalRotation;

    private Vector3 leftFacingDirection;
    private Vector3 rightFacingDirection;

    //3D/2D state
    bool isIn2D = true;
    public LayerMask wall2DLayermask;
    public LayerMask wall3DLayerMask;
    public LayerMask obstacleLayerMask;
    [SerializeField] Camera m_CurrCamera;
    [SerializeField] float m_OcclusionAngle;
    Vector3 m_WorldPosition3D;
    Wall2D m_CurrWall2D;
    Wall3D m_CurrWall3D;
    Vector3 m_WorldTopRight;
    Vector3 m_WorldTopLeft;
    Vector3 m_WorldBottomRight;
    Vector3 m_WorldBottomLeft;

    //Move Skia with shadows
    public bool moveWithShadow;
    public Collider2D testCollider;
    public float ratio;

    public GameObject particleEffect;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        playerCenter = GetComponent<BoxCollider2D>().offset;
        playerSize = GetComponent<BoxCollider2D>().size;
        rayboxSize = new Vector2(playerSize.x - rayboxDistance, rayboxDistance);
        squishDistance = playerSize.x * 0.5f;

        leftFacingDirection = new Vector3(skiaModel.localScale.x, skiaModel.localScale.y, -skiaModel.localScale.z);
        rightFacingDirection = new Vector3(skiaModel.localScale.x, skiaModel.localScale.y, skiaModel.localScale.z);

        collider = GetComponent<BoxCollider2D>();
        colliderCenter = collider.offset;
        colliderSize = collider.size;
        jumping = false;

        m_LevelManager = FindObjectOfType<LevelManager>();
    }

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        //mainCamera = Camera.main;
    }

    private void Update()
    {
        // Find and set its position in 3d space (aka game view)
        SetWorldPosition3D();
        OccludeObjects();

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

        //if (isIn2D)
        //{
        //    //Check if player is squished
        //    bool isSquished = CheckIfSquished();
        //    if (isSquished)
        //    {
        //        ResetPlayer();
        //    }
        //}

        //CheckObjectBlocking();
    }

    private void FixedUpdate()
    {
        if (jumpAction)
        {
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpAction = false;
            grounded = false;
            jumping = true;
            anim.Play("Jump");           
        }
        else
        {
            Vector2 rayboxCenter = (Vector2)transform.position + playerCenter + Vector2.down * (playerSize.y + rayboxSize.y) * 0.5f;
            grounded = (Physics2D.OverlapBox(rayboxCenter, rayboxSize, 0, mask) != null);
        }               

        //SafeColliders();
        SafeTrackPosition();

        //fit collider with anim
        if (!grounded)
        {
            float displace = 0.035f;
            float animDisplace = 0.03f;
            if (rb.velocity.y > 0)
            {
                collider.offset -= new Vector2(0, displace / 2);
                anim.transform.position -= new Vector3(0, animDisplace, 0);
                //collider.size += new Vector2(0.02f, -0.03f);
                collider.size -= new Vector2(0.00f, displace);
            }
            else if (rb.velocity.y < 0 && jumping)
            {
                collider.offset += new Vector2(0, displace / 2);
                anim.transform.position += new Vector3(0, animDisplace, 0);
                collider.size += new Vector2(0, displace);
            }
        }
        else
        {
            if (jumping)
            {
                float difference = collider.offset.y - colliderCenter.y;
                if (difference > 0)
                {
                    transform.position += new Vector3(0, difference, 0);
                }
                jumping = false;
            }
            collider.offset = colliderCenter;
            collider.size = colliderSize;
            anim.transform.position = transform.position;
        }
        playerCenter = collider.offset;
        playerSize = collider.size;
        rayboxSize = new Vector2(playerSize.x - rayboxDistance, rayboxDistance);
        squishDistance = playerSize.x * 0.5f;

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
        if (grounded)
            rb.gravityScale = 0;


        //move Skia with shadow
        if (moveWithShadow)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);
            if ((hit.collider != null) && (hit.collider.tag == "Shadow"))//stand on shadow
            {
                testCollider = hit.collider;

                if (grounded && rb.velocity.x == 0)
                {
                    float displacement = hit.collider.GetComponent<ShadowMoveSkia>().CalulateSkiaDisplacement(hit.point);
                    displacement -= transform.position.x;
                    //transform.position += new Vector3(displacement, 0);
                    //rb.AddForce(new Vector2(displacement, 0) * 200f);
                    displacement /= Time.fixedDeltaTime;
                    rb.velocity = new Vector2(displacement, rb.velocity.y);
                }
                else
                {
                    ratio = hit.collider.GetComponent<ShadowMoveSkia>().UpdateRatio(hit.point);
                }
            }
        }

    }

    void ResetPlayer()
    {
        GameObject cH = GameObject.Find("Ch46");
        SkinnedMeshRenderer cHrenderer = cH.GetComponent<SkinnedMeshRenderer>();
        cHrenderer.enabled = false;
        GameObject pE = Instantiate(particleEffect, transform.position, transform.rotation);
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

    public void SetNewCheckpoint(Vector2 v)
    {
        originalPosition = new Vector3(v.x, v.y, transform.position.z);
        rb.velocity = Vector2.zero;
    }

    void SetWorldPosition3D()
    {
        float sizeX = playerSize.x / 2;
        float sizeY = playerSize.y / 2;
        Vector3 skiaPosition = transform.position + (Vector3)playerCenter;
        m_WorldPosition3D = GetWorldPosition(skiaPosition);
        m_WorldTopRight = GetWorldPosition(skiaPosition + Vector3.right * sizeX + Vector3.up * sizeY);
        m_WorldTopLeft = GetWorldPosition(skiaPosition + Vector3.left * sizeX + Vector3.up * sizeY);
        m_WorldBottomRight = GetWorldPosition(skiaPosition + Vector3.right * sizeX + Vector3.down * sizeY);
        m_WorldBottomLeft = GetWorldPosition(skiaPosition + Vector3.left * sizeX + Vector3.down * sizeY);
        return;
    }

    Vector3 GetWorldPosition(Vector3 point)
    {
        RaycastHit hitInfo;
        bool hitWall2D = Physics.Raycast(point, Vector3.forward, out hitInfo, 100f, wall2DLayermask, QueryTriggerInteraction.Collide);
        if (hitWall2D)
        {
            Wall2D newWall2D = hitInfo.collider.gameObject.GetComponent<Wall2D>();
            if (!m_CurrWall2D || (m_CurrWall2D != newWall2D))
            {
                m_CurrWall2D = newWall2D;
                m_CurrWall3D = m_CurrWall2D.wall3D.GetComponent<Wall3D>();
            }

            point = m_CurrWall2D.SwitchTo3D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point));
        }
        return point;
    }

    private List<Obstacle> obstacleCache = new List<Obstacle>();

    void OccludeObjects()
    {
        Vector3 topRightToCam = m_CurrCamera.transform.position - m_WorldTopRight;
        Vector3 topLeftToCam = m_CurrCamera.transform.position - m_WorldTopLeft;
        Vector3 bottomRightToCam = m_CurrCamera.transform.position - m_WorldBottomRight;
        Vector3 bottomLeftToCam = m_CurrCamera.transform.position - m_WorldBottomLeft;
        Debug.DrawLine(m_WorldTopLeft, m_WorldTopLeft + topLeftToCam);
        Debug.DrawLine(m_WorldTopRight, m_WorldTopRight + topRightToCam);
        Debug.DrawLine(m_WorldBottomRight, m_WorldBottomRight + bottomRightToCam);
        Debug.DrawLine(m_WorldBottomLeft, m_WorldBottomLeft + bottomLeftToCam);
        RaycastHit hitInfo;
        if(Physics.Raycast(m_WorldTopRight, topRightToCam, out hitInfo, Mathf.Infinity, obstacleLayerMask, QueryTriggerInteraction.Collide))
        {
            hitInfo.collider.gameObject.GetComponent<Obstacle>().Occlude();
        }
        if (Physics.Raycast(m_WorldTopLeft, topLeftToCam, out hitInfo, Mathf.Infinity, obstacleLayerMask, QueryTriggerInteraction.Collide))
        {
            hitInfo.collider.gameObject.GetComponent<Obstacle>().Occlude();
        }
        if (Physics.Raycast(m_WorldBottomRight, bottomRightToCam, out hitInfo, Mathf.Infinity, obstacleLayerMask, QueryTriggerInteraction.Collide))
        {
            hitInfo.collider.gameObject.GetComponent<Obstacle>().Occlude();
        }
        if (Physics.Raycast(m_WorldBottomLeft, bottomLeftToCam, out hitInfo, Mathf.Infinity, obstacleLayerMask, QueryTriggerInteraction.Collide))
        {
            hitInfo.collider.gameObject.GetComponent<Obstacle>().Occlude();
        }
        /*foreach (Obstacle obs in obstacleCache)
        {
            obs.Occlude();
        }
        obstacleCache.Clear();

        RaycastHit[] hits = Physics.SphereCastAll(m_WorldPosition3D, m_OcclusionAngle, m_CurrCamera.transform.position - m_WorldPosition3D, 100);
        foreach (RaycastHit hit in hits)
        {
            Obstacle obs = hit.transform.GetComponent<Obstacle>();
            if (obs != null)
            {
                obs.NonOcclude();
                obstacleCache.Add(obs);
            }
        }*/
        /*
        for (int i = 0; i < GameManager.m_Obstacles.Count; i++)
        {
            Vector3 camToPlayer = m_CurrCamera.transform.position - m_WorldPosition3D;
            camToPlayer = camToPlayer.normalized;

            Vector3 camToObstacle = m_CurrCamera.transform.position - GameManager.m_Obstacles[i].transform.position;
            camToObstacle = camToObstacle.normalized;

            float anglePlayerToObstacle = Mathf.Acos(Vector3.Dot(camToPlayer, camToObstacle)) * Mathf.Rad2Deg;
            if (!(anglePlayerToObstacle < m_OcclusionAngle))
            {
                GameManager.m_Obstacles[i].Occlude();
            }
            else
            {
                GameManager.m_Obstacles[i].NonOcclude();
            }

        }
        */
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
        //Debug.DrawRay(transform.position, Vector2.right * squishDistance, Color.green);
        //Debug.DrawRay(transform.position, Vector2.left * squishDistance, Color.green);
        bool ret = false;
        //Debug.Log("checking if squished");
        if (Physics2D.Raycast(transform.position + Vector3.up, Vector2.right, squishDistance, mask)
            && Physics2D.Raycast(transform.position + Vector3.up, Vector2.left, squishDistance, mask))
        {
            //Debug.Log("squished");
            ret = true;
        }
        return ret;
    }

    bool SafeColliders()
    {
        Vector2 center = (Vector2)transform.position + playerCenter;
        float xDist = playerSize.x * 0.5f;
        RaycastHit2D collideLeft = Physics2D.BoxCast(center, new Vector2(rayboxDistance, playerSize.y - rayboxDistance), 0, Vector2.left, xDist, mask); //Physics2D.Raycast(center, Vector2.left, xDist, mask);
        RaycastHit2D collideRight = Physics2D.BoxCast(center, new Vector2(rayboxDistance, playerSize.y - rayboxDistance), 0, Vector2.right, xDist, mask); //Physics2D.Raycast(center, Vector2.right, xDist, mask);
        if (collideLeft && collideRight && collideLeft.transform.position.x < transform.position.x && collideRight.transform.position.x > transform.position.x)
            return true;

        if (collideLeft)
        {
            if (collideLeft.distance < xDist)
            {
                transform.Translate(Vector2.right * (xDist - collideLeft.distance));
            }
        }

        if (collideRight)
        {
            if (collideRight.distance < xDist)
            {
                transform.Translate(Vector2.left * (xDist - collideRight.distance));
            }
        }
        return false;
    }

    Vector2 pastPosition;
    public float thresholdDistance = 0.1f;
    bool trackPosition = false;
    void SafeTrackPosition()
    {
        Vector2 currPosition = (Vector2)transform.position + playerCenter;
        //print(GameManager.edgeCollider2DPool.Count);

        if (ColliderOverlap(currPosition))
        {
            print("skia overlapping shadows");
            //ResetPlayer();
        }    

        ////if (trackPosition)
        ////{
        ////    if (Vector2.Distance(currPosition, pastPosition) > thresholdDistance)
        ////    {
        ////        if (Physics2D.OverlapBox(currPosition, new Vector2(playerSize.x - rayboxDistance, playerSize.y - rayboxDistance), 0, mask))
        ////        //if (Physics2D.OverlapPoint(currPosition, mask))
        ////        {
        ////            transform.Translate((pastPosition - currPosition));
        ////            print("adjust for overlapping shadow");
        ////        }
        ////    }
        ////}
        ////else
        ////{
        ////    trackPosition = true;
        ////}


        //if (Physics2D.OverlapBox(currPosition, new Vector2(playerSize.x / 3, playerSize.y / 3), 0, mask))
        //{
        //    ResetPlayer();
        //    print("reset due to overlapping shadows");
        //}
        ////else if (Physics2D.Raycast(currPosition + playerCenter, Vector2.right, squishDistance, mask)
        ////    && Physics2D.Raycast(currPosition + playerCenter, Vector2.left, squishDistance, mask))
        ////{
        ////    ResetPlayer();
        ////    print("reset due to raycasting squish");
        ////}

        //pastPosition = (Vector2)transform.position + playerCenter;
    }

    bool ColliderOverlap(Vector2 position)
    {
        foreach (EdgeCollider2D collider in m_LevelManager.GetCurrentSegment().GetEdgeColliderPool())
        {
            if (collider.isTrigger)
                continue;

            bool success = true;
            Vector2[] cPoints = collider.points;
            int k = 0;
            for (int i = 0; i < cPoints.Length - 1; i++)
            {
                float dp = Vector2.Dot(new Vector2(cPoints[i].y - cPoints[i + 1].y, cPoints[i + 1].x - cPoints[i].x), position - cPoints[i]);
                if (k == 0)
                    k = dp > 0 ? 1 : -1;
                else if ((dp > 0 ? 1 : -1) != k)
                {
                    success = false;
                    break;
                }
            }
            if (success)
            {
                //print("skia overlapping shadow " + collider);
                return true;
            }
        }

        return false;
    }

    private void ChangeCollider(Vector2 center, float width, float height)
    {
        collider.offset = center;
        collider.size = new Vector2(width, height);
    }

    public void Disable()
    {
        rb.velocity = Vector2.zero;
        anim.SetBool("IsRunning", false);
        enabled = false;
    }
}
