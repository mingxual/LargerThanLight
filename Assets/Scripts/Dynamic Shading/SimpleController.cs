using UnityEngine;

public class SimpleController : MonoBehaviour
{
    #region Skia Components To Be Set Before Runtime
    [SerializeField] private Transform m_SkiaModelTransform;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Camera m_CurrentGameCamera;
    [SerializeField] private GameObject m_DeathParticleEffect;
    [SerializeField] private GameObject m_DraggingParticleEffect;
    #endregion

    #region Skia Components To Be Set At Beginning of Runtime
    private SkinnedMeshRenderer m_SkinnedMeshRenderer;
    private Rigidbody2D m_Rigidbody2D;
    private BoxCollider2D m_BoxCollider2D;
    private SkiaVignette m_SkiaVignette;
    #endregion

    #region Skia Inspector Variables To Be Set Before Runtime
    [SerializeField] private float m_MoveSpeed = 10;
    [SerializeField] private float m_JumpSpeed = 10;
    [SerializeField] private float m_FallingStateGravityMultiplier = 3.5f;
    [SerializeField] private float m_DefaultGravityMultiplier = 2f;
    [SerializeField] private float m_RayboxDistance = 0.05f;
    [SerializeField] private float m_AnimationGameObjectShiftUp;
    [SerializeField] private float m_AnimationGameObjectShiftDown;
    [SerializeField] private float m_OcclusionAngle;
    [SerializeField] private float m_JumpGraceDelay;
    [SerializeField] private LayerMask m_ObstacleLayerMask;
    [SerializeField] private LayerMask m_Wall2DLayerMask;
    [SerializeField] private ContactFilter2D m_GroundedContactFilter;
    [SerializeField] private ContactFilter2D m_UngroundedContactFilter;
    [SerializeField] private bool m_IsMovingWithShadow;
    #endregion

    #region Skia State Variables Updated During Runtime 
    /* Skia States */
    private bool m_IsDead;
    private bool m_HasJumped;
    private bool m_IsGrounded;
    private bool m_IsJumping;
    private bool m_CanJump;

    /* Keep track if any key is pressed during each frame */
    private bool m_KeysPressed;
    #endregion

    #region Skia Information To Be Set At The Beginning of Runtime
    private LightController m_LuxReference;
    private Vector2 m_PlayerCenter; // The "center" point of Skia
    private Vector2 m_PlayerSize; 
    private Vector2 m_BoxCollider2DCenter; // The "center" point of Skia's 2D box collider
    private Vector2 m_BoxCollider2DSize;
    private Vector3 m_LeftFacingDirection;
    private Vector3 m_RightFacingDirection;
    #endregion

    #region Skia Information Updated During Runtime
    private Vector3 m_SpawnPosition2D;
    private Vector3 m_WorldPosition3D;
    private float m_MovementDirection;
    private Collider2D m_CurrentlyHitCollider;
    private GameObject m_LastGroundedGameObject;
    private Wall2D m_CurrWall2D;
    private float m_CurrentJumpGraceTimer;
    //private float m_PreviousSquishValueVignette = 1.0f;
    private float m_ShadowDistanceRatio;
    #endregion

    #region Manager References
    GameManager m_GameManager;
    #endregion

    #region Anything Debugging Related
    public bool m_MichaelsDebuggingFlag;
    public bool mehaIsStupid = false;
    #endregion

    private void Awake()
    {
        // Get components
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_BoxCollider2D = GetComponent<BoxCollider2D>();
        m_SkiaVignette = GetComponent<SkiaVignette>();
        m_SkinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        // Get managers
        m_GameManager = FindObjectOfType<GameManager>();
        if (m_GameManager)
        {
            // Get reference to Lux
            if (m_GameManager.lux) m_LuxReference = m_GameManager.lux.GetComponent<LightController>();
        }

        m_SkinnedMeshRenderer.updateWhenOffscreen = true; // Allow the AABB of its animated mesh to update in order to keep bounding it

        // Setup initial information about Skia
        m_BoxCollider2DCenter = m_BoxCollider2D.offset;
        m_BoxCollider2DSize = m_BoxCollider2D.size;
        m_PlayerCenter = m_BoxCollider2D.offset;
        m_BoxCollider2DSize = m_BoxCollider2D.size;

        // Set jumping state
        m_IsJumping = false;
    }

    void Start()
    {
        m_SpawnPosition2D = transform.position; // Set Skia's 2D spawn position

        /* Set Skia's facing direction */
        m_LeftFacingDirection = new Vector3(m_SkiaModelTransform.localScale.x, m_SkiaModelTransform.localScale.y, -m_SkiaModelTransform.localScale.z);
        m_RightFacingDirection = new Vector3(m_SkiaModelTransform.localScale.x, m_SkiaModelTransform.localScale.y, m_SkiaModelTransform.localScale.z);
    }

    private void Update()
    {
        // Update time until player can jump again
        if(!m_IsGrounded)
        {
            if (m_CurrentJumpGraceTimer > 0)
                m_CurrentJumpGraceTimer -= Time.deltaTime;
            else
                m_CanJump = false;
        }

        SetWorldPosition3D(); // Set Skia's position in the 3D world
        OccludeObjects(); // Fade any obstacles in front of Skia

        //-----INPUT CONTROLS-----
        m_MovementDirection = 0;
        m_KeysPressed = false;
        if (Input.GetKey(KeyCode.A)) // Move left
        {
            m_MovementDirection -= 1;
            m_KeysPressed = true;
        }
        if (Input.GetKey(KeyCode.D)) // Move right
        {
            m_MovementDirection += 1;
            m_KeysPressed = true;
        }
        if ((Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.Space)) && m_CanJump)
        {
            m_HasJumped = true;
        }
        //--END OF INPUT CONTROLS--

        //-----DEBUG CONTROLS-----
        if (m_MichaelsDebuggingFlag)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                ResetSkia();
            }
        }
        //--END OF DEBUG CONTROLS--
    }

    private void FixedUpdate()
    {
        if (m_IsDead)
        {
            Debug.Log("Dead state");
            return;
        }
        // Displace Skia from any one-sided collisions, don't check if dead
        CheckCollisions(false); 

        // Set Skia's states and variables after jumping
        if (m_HasJumped)
        {
            m_Rigidbody2D.velocity = Vector3.zero; // Set Skia's velocity to 0
            m_Rigidbody2D.AddForce(Vector2.up * m_JumpSpeed, ForceMode2D.Impulse); // Force added to Skia to make her jump

            // Set jump/grounded states
            m_HasJumped = false; 
            m_CanJump = false;
            m_IsGrounded = false;
            m_IsJumping = true;

            // Play Skia's jump animation
            m_Animator.Play("Jump", -1, 0f);
        }
        else
        {        
            m_IsGrounded = m_BoxCollider2D.IsTouching(m_GroundedContactFilter); // Get Skia's grounded status 

            // Determine whether Skia can jump again 
            if (m_IsGrounded && !m_IsJumping)
            {
                m_CanJump = true;
                m_CurrentJumpGraceTimer = m_JumpGraceDelay; // Reset the grace timer until Skia can jump again
            }
        }

        // Check if Skia is jumping
        if (m_IsJumping)
        {
            Bounds bounds = m_SkinnedMeshRenderer.bounds; // Get AABB bounds for box bounding Skia's animated mesh

            // Adjust Skia's collider based off on current bounds
            m_BoxCollider2D.size = new Vector2(m_BoxCollider2D.size.x, bounds.size.y - 0.2f);
            m_BoxCollider2D.offset = new Vector2(m_BoxCollider2D.offset.x, bounds.extents.y - 0.1f);

            // Move animator's game object based on Y velocity
            if (m_Rigidbody2D.velocity.y > 0)
                m_Animator.transform.position -= Vector3.up * m_AnimationGameObjectShiftUp;
            else if (m_Rigidbody2D.velocity.y < 0)
                m_Animator.transform.position += Vector3.up * m_AnimationGameObjectShiftDown;

            // Check for negative velocity to set jumping state to false
            if (m_BoxCollider2D.IsTouching(m_UngroundedContactFilter) && m_Rigidbody2D.velocity.y <= 0)
            {
                m_IsJumping = false;
            }
        }

        // Check if Skia becomes grounded after being in a jumping state
        if (m_IsGrounded)
        {
            // Reset Skia's collider to original position and size
            m_BoxCollider2D.offset = m_BoxCollider2DCenter;
            m_BoxCollider2D.size = m_BoxCollider2DSize;

            // Reset animator object's position
            m_Animator.transform.position = transform.position;

            // Check for negative velocity to set jumping state to false
            if (m_Rigidbody2D.velocity.y <= 0)
            {
                m_IsJumping = false;
            }
        }

        // Update player center and size to the bounding box
        m_PlayerCenter = m_BoxCollider2D.offset;
        m_PlayerSize = m_BoxCollider2D.size;

        // Set Skia's running animation if she's moving horizontally
        m_Rigidbody2D.velocity = new Vector2(m_MovementDirection * m_MoveSpeed, m_Rigidbody2D.velocity.y);
        if(m_MovementDirection == 0)
        {
            if (!m_KeysPressed)
                m_Animator.SetBool("IsRunning", false);
        }
        else
        {
            if (mehaIsStupid == false)
            {
                // Set Skia's facing direction
                m_SkiaModelTransform.localScale = m_MovementDirection > 0 ? m_RightFacingDirection : m_LeftFacingDirection;
                m_Animator.SetBool("IsRunning", true);
            }
            if (mehaIsStupid == true)
            {
                // Set Skia's facing direction
                m_SkiaModelTransform.localScale = m_MovementDirection > 0 ? m_LeftFacingDirection : m_RightFacingDirection;
                m_Animator.SetBool("IsRunning", true);
            }
        }

        // Detemine how fast Skia should fall/jump
        if (m_Rigidbody2D.velocity.y < 0)
            m_Rigidbody2D.gravityScale = m_FallingStateGravityMultiplier;
        else
            m_Rigidbody2D.gravityScale = m_DefaultGravityMultiplier;
        if (m_IsGrounded)
            m_Rigidbody2D.gravityScale = 1;


        // Check if Skia can move with shadow
        if (m_IsMovingWithShadow)
        {
            // Check what kind of obstacle Skia collides with
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.5f);
            if ((hit.collider != null) && (hit.collider.tag == "Shadow"))
            {
                m_CurrentlyHitCollider = hit.collider; // Save collider that Skia is currently on contact with

                // Check if Skia is grounded, not moving, and is standing on the shadow casted by the same game object during the last FixedUpdate
                if (m_IsGrounded && (m_Rigidbody2D.velocity.x == 0) && (m_LastGroundedGameObject == m_CurrentlyHitCollider.gameObject))
                {
                    // Displace Skia's horizontal position with velocity
                    //print("landing on same obstacle" + m_ShadowDistanceRatio);
                    float displacement = hit.collider.GetComponent<ShadowMoveSkia>().CalulateSkiaDisplacement(hit.point);
                    displacement -= transform.position.x;
                    displacement /= Time.fixedDeltaTime;
                    m_Rigidbody2D.velocity = new Vector2(displacement, m_Rigidbody2D.velocity.y);
                }
                else
                {
                    m_ShadowDistanceRatio = hit.collider.GetComponent<ShadowMoveSkia>().UpdateRatio(hit.point);
                    //print("different obstacle, moving to ratio" + m_ShadowDistanceRatio);
                    // Update a new game object that doesn't match the last collider's game object

                    m_LastGroundedGameObject = m_CurrentlyHitCollider.gameObject;
                }
            }
            else
            {
                m_LastGroundedGameObject = null;
            }
        }

        // Displace Skia from any one-sided collisions, and check how much she's being squished
        float death = CheckCollisions(true);
        if (death == -1) // If squished (aka dead), reset Skia
        {
            if (m_LuxReference.LightActiveStatus())
            {
                if (m_LuxReference.IsMoving())
                {
                    SkiaDeath();
                }
            }
            else
            {
                SkiaDeath();
            }
        }
        else
        {
            // Adjust vignette effect depending on how much she's squished
            m_SkiaVignette.squishStatus = death * 0.6f;
            Vector2 skiaScreenPos = m_CurrentGameCamera.WorldToScreenPoint(m_WorldPosition3D);
            skiaScreenPos.x /= Screen.width;
            skiaScreenPos.y /= Screen.height;
            m_SkiaVignette.SetVignetteCenter(skiaScreenPos);
            LevelManager.Instance.SlowTime(death > 0.5f);

            /*if (death > m_PreviousSquishValueVignette)
            {
                m_SkiaVignette.squishStatus = death * 0.6f;
                Vector2 skiaScreenPos = m_CurrentGameCamera.WorldToScreenPoint(m_WorldPosition3D);
                skiaScreenPos.x /= Screen.width;
                skiaScreenPos.y /= Screen.height;
                m_SkiaVignette.SetVignetteCenter(skiaScreenPos);
                LevelManager.Instance.SlowTime(true);
                m_PreviousSquishValueVignette = death;
            }
            else
            {
                m_SkiaVignette.squishStatus = 0f;
                LevelManager.Instance.SlowTime(false);
                m_PreviousSquishValueVignette = 0f;
            }*/
        }
    }

    // Get Skia's 3D world position
    public Vector3 GetWorldPosition3D()
    {
        return m_WorldPosition3D;
    }

    // Set light status
    public void SetLightStatus(float status)
    {
        if(status == -1)
        {
            SkiaDeath();
            return;
        }

        if(m_SkiaVignette)
            m_SkiaVignette.lightStatus = status;
    }

    public void SkiaDeath()
    {
        Debug.Log("Skia Death");
        Vector3 oldPosition = transform.position; // Store Skia's old position before moving her
        m_SkinnedMeshRenderer.enabled = false; // Turn off mesh renderer
        ResetSkia(); // Reset Skia, moving her to the new spawn point
        m_IsDead = true; // Set dead status
        Destroy(Instantiate(m_DeathParticleEffect, oldPosition, transform.rotation), 1.0f); // Instantiate the instant death effect
        GameObject dragPart = Instantiate(m_DraggingParticleEffect, oldPosition, transform.rotation); // Instantiate the dragging particle effect that will move towards spawn
        dragPart.GetComponent<MoveToOrigin>().target = m_SpawnPosition2D; // Set dragging particle effect's target location to move toward
        dragPart.GetComponent<MoveToOrigin>().thePlayerMesh = m_SkinnedMeshRenderer; // Set Skia's mesh renderer to drag particle so it can turn on Skia's mesh once it reaches location

        /*MoveToOrigin moveToOrigin = dragPart.GetComponent<MoveToOrigin>();
        moveToOrigin.target = m_SpawnPosition2D; // Set dragging particle effect's target location to move toward
        moveToOrigin.thePlayerMesh = m_SkinnedMeshRenderer; // Set Skia's mesh renderer to drag particle so it can turn on Skia's mesh once it reaches location
        moveToOrigin.m_SimpleController = this;
        enabled = false; // Disable controls*/
    }

    /// <summary>
    /// Resets Skia to current spawnpoint
    /// Updated 2/14 in use
    /// </summary>
    private void ResetSkia()
    {
        // Find a spawnable 2D position
        bool spawnable = SCManager.Instance.RaycastSpawnpoint(out Vector2 spawnpoint);
        if (!spawnable)
        {
            m_LuxReference.ResetLux();
        }

        // Set Skia's location to 2D position
        Vector3 point = spawnpoint;
        point.z = transform.position.z;
        transform.position = point;
        m_SpawnPosition2D = point; // Update spawn position location

        // Reset velocity to 0
        m_Rigidbody2D.velocity = Vector2.zero;
    }

    // Store the 2D coordinates of her spawn location
    public void SetNewCheckpoint(Vector2 v)
    {
        m_SpawnPosition2D = new Vector3(v.x, v.y, transform.position.z);
        m_Rigidbody2D.velocity = Vector2.zero;
    }
     
    // Sets Skia's 3D world position
    void SetWorldPosition3D()
    {
        Vector3 skiaCenterPosition = transform.position + (Vector3)m_PlayerCenter;
        m_WorldPosition3D = GetWorldPosition3D(skiaCenterPosition);
        return;
    }

    // Finds and sets Skia's 3D world position
    Vector3 GetWorldPosition3D(Vector3 position)
    {
        //Check which 3D wall Skia is at
        RaycastHit hitInfo;
        bool hitWall2D = Physics.Raycast(position, Vector3.forward, out hitInfo, 100f, m_Wall2DLayerMask, QueryTriggerInteraction.Collide);
        if (hitWall2D)
        {
            Wall2D newWall2D = hitInfo.collider.gameObject.GetComponent<Wall2D>();
            if (!m_CurrWall2D || (m_CurrWall2D != newWall2D))
            {
                m_CurrWall2D = newWall2D;
            }

            // Get 3D position coordinates
            position = m_CurrWall2D.SwitchTo3D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point));
        }
        return position;
    }

    private RaycastHit[] occlusionBuffer = new RaycastHit[20];

    // Fade any obstacles in front of Skia in-game
    void OccludeObjects()
    {
        Vector3 centerToCam = m_CurrentGameCamera.transform.position - m_WorldPosition3D; // "Skia's 3D position to current camera's position" vector

        /* Find all obstacles that are in front of Skia */
        int k = Physics.SphereCastNonAlloc(m_WorldPosition3D, m_OcclusionAngle, centerToCam.normalized, occlusionBuffer, 100f, m_ObstacleLayerMask, QueryTriggerInteraction.Collide);
        for(int i = 0; i < k; i++)
        {
            SCObstacle sCObstacle = occlusionBuffer[i].collider.GetComponent<SCObstacle>();
            if (sCObstacle)
                sCObstacle.Occlude(); // Fade obstacles
        }
    }

    // Displaces Skia if she's colliding with an obstacle
    // Or checks how much she's being squished by the checkingIfDead variable
    float CheckCollisions(bool checkingIfDead)
    {
        Vector2 skiaCenterPosition = (Vector2)transform.position + m_PlayerCenter;
        float xDist = m_PlayerSize.x * 0.5f;
        float yDist = m_PlayerSize.y * 0.5f;

        /* Check for shadow/obstacle collisions on each side of Skia */
        Physics2D.queriesHitTriggers = false;
        Vector2 verticalSize = Vector2.right * m_RayboxDistance + Vector2.up * m_PlayerSize.y * 0.9f;
        Vector2 horizontalSize = Vector2.right * m_PlayerSize.x * 0.9f + Vector2.up * m_RayboxDistance;
        RaycastHit2D collideLeft = Physics2D.BoxCast(skiaCenterPosition, verticalSize, 0, Vector2.left, 2 * xDist, m_ObstacleLayerMask); //Physics2D.Raycast(center, Vector2.left, xDist, mask);
        RaycastHit2D collideRight = Physics2D.BoxCast(skiaCenterPosition, verticalSize, 0, Vector2.right, 2 * xDist, m_ObstacleLayerMask); //Physics2D.Raycast(center, Vector2.right, xDist, mask);
        RaycastHit2D collideTop = Physics2D.BoxCast(skiaCenterPosition, horizontalSize, 0, Vector2.up, yDist, m_ObstacleLayerMask);
        RaycastHit2D collideBottom = Physics2D.BoxCast(skiaCenterPosition, horizontalSize, 0, Vector2.down, yDist, m_ObstacleLayerMask);
        Physics2D.queriesHitTriggers = true;

        if(checkingIfDead)
        {
            // Check if Skia is being squished from the left and right
            if (collideLeft && collideRight)
            {
                if(Mathf.Abs(Vector2.SignedAngle(Vector2.right, collideLeft.normal)) < m_GroundedContactFilter.minNormalAngle &&
                    Mathf.Abs(Vector2.SignedAngle(Vector2.right, collideRight.normal)) > m_GroundedContactFilter.maxNormalAngle)
                {
                    // Check how much Skia is being squished
                    if (collideLeft.distance < xDist - m_RayboxDistance && collideRight.distance < xDist - m_RayboxDistance)
                        return -1;
                    else
                    {
                        return 1 - (collideLeft.distance + collideRight.distance - 2 * xDist) / (2 * xDist + m_RayboxDistance);
                    }
                }
            }

            // Check if Skia is being squished from the top and left
            if (collideTop && collideBottom)
            {
                // Check how much Skia is being squished
                if (collideTop.distance < yDist - m_RayboxDistance && collideBottom.distance < yDist - m_RayboxDistance)
                    return -1;
                else
                    return 1 - (collideTop.distance + collideBottom.distance - 2 * yDist) / m_RayboxDistance;
            }
        }

        // Check for a left-side collision and displace Skia by how much she intersects the collider
        if (collideLeft)
        {
            if (Mathf.Abs(Vector2.SignedAngle(Vector2.right, collideLeft.normal)) < m_GroundedContactFilter.minNormalAngle)
            {
                if (collideLeft.distance < xDist)
                {
                    transform.Translate(Vector2.right * (xDist - collideLeft.distance));
                }
            }
        }

        // Check for a right-side collision and displace Skia by how much she intersects the collider
        if (collideRight)
        {
            if (Mathf.Abs(Vector2.SignedAngle(Vector2.right, collideRight.normal)) > m_GroundedContactFilter.maxNormalAngle)
            {
                if (collideRight.distance < xDist)
                {
                    transform.Translate(Vector2.left * (xDist - collideRight.distance));
                }
            }
        }
        return 0;
    }

    // Return dead status
    public bool IsDead()
    {
        return m_IsDead;
    }

    // Set dead status
    public void SetDeadStatus(bool val)
    {
        m_IsDead = val;
    }

    // Disable and freeze Skia
    public void Disable()
    {
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Animator.SetBool("IsRunning", false);
        enabled = false;
    }
}
