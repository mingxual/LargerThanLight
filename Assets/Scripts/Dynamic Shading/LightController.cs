using UnityEngine;

public class LightController : MonoBehaviour
{
    #region Lux Components To Be Set Before Runtime
    [SerializeField] private Animator m_Animator;
    [SerializeField] private Transform m_LuxModelTransform;
    [SerializeField] private SCLight m_SCLight;
    #endregion

    #region Lux Components To Be Set At The Beginning Of Runtime
    private Rigidbody m_Rigidbody;
    private bool m_AlsoVerticalMovement; // Determines whether Lux is restricted to horizontal movement or not
    #endregion

    #region Lux Inspector Variables To Be Set Before Runtime
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] Vector3 m_ForwardDirection;
    [SerializeField] private bool m_CheckIsBlockedAtFeet = true; // Check if feet hit colliders to stop Lux animation
    [SerializeField] private float m_RaycastMovementLength = 1.0f; // Distance to check where Lux hits colliders to stop his animation
    #endregion

    #region Lux State Variables Updated During Runtime
    public static bool m_LuxKeyPressed;
    private bool m_StoppedByCollider; // Check if Lux is stopped by a collider
    private bool m_IsMoving = false;
    #endregion

    #region Lux Information Updated During Runtime
    private Vector3 m_MovementDirection;
    private Vector3 m_AttemptedLookDirection; // The direction player wants Lux to look at
    private Vector3 m_RightDirection; // Lux's right direction
    #endregion

    private void Awake()
    {
        // Get compnents
        m_Rigidbody = GetComponent<Rigidbody>();

        // Set Lux's forward vector
        SetForwardDirection(m_ForwardDirection);
    }

    private void OnEnable()
    {
        m_AlsoVerticalMovement = true; // Enable vertical movement in addition to horizontal
    }

    void Update()
    {
        m_IsMoving = false;
        m_StoppedByCollider = false;
        m_MovementDirection = Vector3.zero;
        m_AttemptedLookDirection = Vector3.zero;
        m_LuxKeyPressed = false;

        //---------INPUT CONTROLS------------
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_MovementDirection -= m_RightDirection;
            m_LuxKeyPressed = true;
            m_IsMoving = true;

            StopMovementIfBlocked();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            m_MovementDirection += m_RightDirection;
            m_LuxKeyPressed = true;
            m_IsMoving = true;

            StopMovementIfBlocked();
        }

        // Lux's horizontal movement input
        if (m_AlsoVerticalMovement)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                m_MovementDirection += m_ForwardDirection;
                m_LuxKeyPressed = true;
                m_IsMoving = true;

                StopMovementIfBlocked();
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                m_MovementDirection -= m_ForwardDirection;
                m_LuxKeyPressed = true;
                m_IsMoving = true;

                StopMovementIfBlocked();
            }
        }

        //------END OF INPUT CONTROLS------
    }

    private void FixedUpdate()
    {
        // Stop moving Lux if there's no movement
        if(m_MovementDirection == Vector3.zero)
        {
            m_Rigidbody.velocity = Vector3.down;
            if (!m_LuxKeyPressed)
               m_Animator.SetBool("Moving", false);
            else if (m_StoppedByCollider)
            {
                // Don't let Lux move if he's moving into a collider, but at least let him look in the direction it's blocked
                m_Animator.SetBool("Moving", false);
                m_LuxModelTransform.LookAt(m_LuxModelTransform.position + m_AttemptedLookDirection);
            }
            return;
        }

        // Set Lux's speed if moving
        m_Rigidbody.velocity = m_MovementDirection.normalized * m_MoveSpeed;
        if (m_MovementDirection.y == 0)
        {
            m_Rigidbody.velocity = Vector3.right * m_Rigidbody.velocity.x + Vector3.down + Vector3.forward * m_Rigidbody.velocity.z; // Vector3(m_Rigidbody.velocity.x, -1f, m_Rigidbody.velocity.z);
        }

        // Make sure Lux does not look upward or downward
        Vector3 sightDirection = m_MovementDirection;
        sightDirection.y = 0f;
        if (sightDirection != Vector3.zero)
        {
            m_LuxModelTransform.LookAt(m_LuxModelTransform.position + sightDirection);
            m_Animator.SetBool("Moving", true);
        }
    }

    // Will check if Lux hits an object and will stop him from further moving in that direction
    private void StopMovementIfBlocked()
    { 
        // Determine origin of ray for raycasting against any collider
        Vector3 rayOrigin = m_LuxModelTransform.position;
        if (!m_CheckIsBlockedAtFeet) rayOrigin += Vector3.up * 0.5f;
        else rayOrigin += Vector3.up * 0.01f;

        // Check if we hit any collider
        if (Physics.Raycast(rayOrigin, m_MovementDirection, m_RaycastMovementLength))
        {
            m_IsMoving = false;
            m_AttemptedLookDirection = m_MovementDirection;
            m_MovementDirection = Vector3.zero;
            m_StoppedByCollider = true;
        }
    }

    // Set the forward direction of Lux
    public void SetForwardDirection(Vector3 dir)
    {
        m_ForwardDirection = dir;
        m_RightDirection = Vector3.Cross(Vector3.up, m_ForwardDirection);
    }

    // Stop Lux moving animation and speed
    public void DisableLuxMovement()
    {
        m_MovementDirection = Vector3.zero;
        m_Animator.SetBool("Moving", false);
        m_Rigidbody.velocity = Vector3.zero;
    }

    // Reset Lux's position to a spawn point, if any
    public void ResetLux()
    {
        if (!m_SCLight.active) return;
        Transform spawnpoint = LevelManager.Instance.GetLuxSpawnpoint();
        if(spawnpoint)
        {
            transform.position = spawnpoint.position;
            m_Rigidbody.velocity = Vector3.zero;
        }
    }

    // Return status of light script attached to Lux
    public bool LightActiveStatus()
    {
        return m_SCLight.active;
    }

    // Return whether Lux is moving
    public bool IsMoving()
    {
        return m_IsMoving;
    }

    // By default, Lux is assumed to only have horizontal movement
    // Assign whether he can move vertically or not
    public void SetVerticalMovement(bool val)
    {
        m_AlsoVerticalMovement = val;
    }

    // Set the length amount for raycasting against colliders to check if Lux's animation should stop if there's a hit
    public void SetRaycastMovementLength(float length)
    {
        m_RaycastMovementLength = length;
    }
}
