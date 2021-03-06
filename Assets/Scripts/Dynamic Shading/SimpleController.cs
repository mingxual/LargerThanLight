﻿using System.Collections;
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
    public bool iDied;

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
    private GameObject m_LastGroundedGameObject;

    public GameObject particleEffect;
    GameManager m_GameManager;
    LightController m_LuxReference;

    public ContactFilter2D groundedContactFilter;

    public bool michaelsdebuggingflag;
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

        skiaVignette = GetComponent<SkiaVignette>();
    }

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        m_GameManager = FindObjectOfType<GameManager>();
        if(m_GameManager)
        {
            if (m_GameManager.lux) m_LuxReference = m_GameManager.lux.GetComponent<LightController>();
        }
        //mainCamera = Camera.main;
    }

    private void Update()
    {

        // Find and set its position in 3d space (aka game view)
        SetWorldPosition3D();
        OccludeObjects();
        //Debug.DrawRay(m_WorldPosition3D, Camera.main.transform.position - m_WorldPosition3D);

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

        if (michaelsdebuggingflag)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                ResetSkia();
            }
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

    Collider2D[] cols = new Collider2D[10];

    private void FixedUpdate()
    {
        SafeColliders(true);

        if (jumpAction)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpAction = false;
            grounded = false;
            jumping = true;
            anim.Play("Jump");           
        }
        else
        {
            //Vector2 rayboxCenter = (Vector2)transform.position + playerCenter + Vector2.down * (playerSize.y + rayboxSize.y) * 0.5f;
            //int count = Physics2D.OverlapBoxNonAlloc(rayboxCenter, rayboxSize, 0, cols, mask);
            //grounded = false;
            //for (int i = 0; i < count; i++)
            //{
            //    if (!cols[i].isTrigger)
            //    {
            //        grounded = true;
            //        break;
            //    }
            //}
            
            grounded = collider.IsTouching(groundedContactFilter);
        }

        //SafeColliders();
        //SafeTrackPosition();

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

                if (grounded && rb.velocity.x == 0 && m_LastGroundedGameObject == testCollider.gameObject)
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
                    m_LastGroundedGameObject = testCollider.gameObject;
                    ratio = hit.collider.GetComponent<ShadowMoveSkia>().UpdateRatio(hit.point);
                }
            }
        }

        float death = SafeColliders(false);
        if (death == -1)
        {
            if (m_LuxReference.LightActive())
            {
                if (m_LuxReference.isMoving)
                {
                    ResetSkia();
                }
            }
            else
            {
                ResetSkia();
            }
        }
        else
        {
            skiaVignette.squishStatus = death;
        }
    }

    public Vector3 GetWorldPosition()
    {
        return m_WorldPosition3D;
    }

    private SkiaVignette skiaVignette;

    public void SetLightStatus(float status)
    {
        if(status == -1)
        {
            print("skia dead");
            ResetSkia();
            return;
        }
        if(skiaVignette)
            skiaVignette.lightStatus = status;
    }

    /// <summary>
    /// Resets Skia to current spawnpoint
    /// Updated 2/14 in use
    /// </summary>
    public void ResetSkia()
    {
        iDied = true;
        bool spawnable = SCManager.Instance.RaycastSpawnpoint(out Vector2 spawnpoint);
        if (!spawnable)
        {
            //print("Cannot spawn at " + spawnpoint);
            LevelManager.Instance.lux.GetComponent<LightController>().ResetLux();
        }
        Vector3 point = spawnpoint;
        point.z = transform.position.z;
        transform.position = point;
        rb.velocity = Vector2.zero;
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

    private RaycastHit[] occlusionBuffer = new RaycastHit[20];
    void OccludeObjects()
    {
        Debug.DrawLine(m_WorldPosition3D, m_CurrCamera.transform.position);
        Vector3 centerToCam = m_CurrCamera.transform.position - m_WorldPosition3D;
        int k = Physics.SphereCastNonAlloc(m_WorldPosition3D, m_OcclusionAngle, centerToCam.normalized, occlusionBuffer, 100f, obstacleLayerMask, QueryTriggerInteraction.Collide);
        for(int i = 0; i < k; i++)
        {
            SCObstacle sCObstacle = occlusionBuffer[i].collider.GetComponent<SCObstacle>();
            if (sCObstacle)
                sCObstacle.Occlude();
        }
    }

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

    float SafeColliders(bool flagInitial)
    {
        Vector2 center = (Vector2)transform.position + playerCenter;
        float xDist = playerSize.x * 0.5f;
        float yDist = playerSize.y * 0.5f;

        Physics2D.queriesHitTriggers = false;
        RaycastHit2D collideLeft = Physics2D.BoxCast(center, new Vector2(rayboxDistance, playerSize.y * 0.9f), 0, Vector2.left, xDist, mask); //Physics2D.Raycast(center, Vector2.left, xDist, mask);
        RaycastHit2D collideRight = Physics2D.BoxCast(center, new Vector2(rayboxDistance, playerSize.y * 0.9f), 0, Vector2.right, xDist, mask); //Physics2D.Raycast(center, Vector2.right, xDist, mask);
        RaycastHit2D collideTop = Physics2D.BoxCast(center, new Vector2(playerSize.x * 0.9f, rayboxDistance), 0, Vector2.up, yDist, mask);
        RaycastHit2D collideBottom = Physics2D.BoxCast(center, new Vector2(playerSize.x * 0.9f, rayboxDistance), 0, Vector2.down, yDist, mask);
        Physics2D.queriesHitTriggers = true;

        //if (collideLeft && collideRight && collideLeft.transform.position.x < transform.position.x && collideRight.transform.position.x > transform.position.x)
        if(!flagInitial)
        {
            if (collideLeft && collideRight)
            {
                if (collideLeft.distance < xDist - rayboxDistance && collideRight.distance < xDist - rayboxDistance)
                    return -1;
                else if (collideLeft.distance < xDist && collideRight.distance < xDist)
                    return 0.4f;
            }
            if (collideTop && collideBottom)
            {
                if (collideTop.distance < yDist - rayboxDistance && collideBottom.distance < yDist - rayboxDistance)
                    return -1;
                else if (collideTop.distance < yDist && collideBottom.distance < yDist)
                    return 0.3f;
            }
        }

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
        return 0;
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
        if(m_LevelManager)
        {
            /*foreach (EdgeCollider2D collider in m_LevelManager.GetCurrentSegment().GetEdgeColliderPool())
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
            }*/
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
