using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The script would be attached to 3D obstacles that would generate 2D shadows
public class ShadowEventHandler : MonoBehaviour
{
    // The event name registered in the eventsmanager
    [SerializeField] string mEventKey;

    // The gameobject that it concerns
    [SerializeField] GameObject mContactObject;

    // true when trigger, false when collide
    [SerializeField] bool isEventTrigger;

    // true when need to press space to invoke, false when not
    [SerializeField] bool isSpaceInputNeeded;

    // true when invoke many times, false when invoke only once
    [SerializeField] bool isReusable;

    // if the current object is a spawnpoint
    [SerializeField] bool isSpawnPointTrigger;

    // private variables
    private bool m_Contacted; // Variable to track whether its shadow collision is contacting with the target object 
    private bool m_UpdateThisFrame; // Variable to denote whether in the current frame, m_Contacted is set to true by its shadow collision
    private bool m_TriggeredThisContact; // Variable to track whether the event has been triggered (useful only in invoke multiple times)
    // private bool m_SpawnPointChange; // Variable to check the current spawnpoint script has already been triggered

    private void Start()
    {
        m_Contacted = false;
        m_UpdateThisFrame = false;
        m_TriggeredThisContact = false;
        // m_SpawnPointChange = false;
    }

    private void Update()
    {
        if (m_Contacted && !m_TriggeredThisContact)
        {
            // Two cases
            // one does not need space input, the other does
            // if (!isSpawnPointTrigger)
            //{
                if (!isSpaceInputNeeded || Input.GetAxis("Interaction") > 0.8f)
                {
                    EventsManager.instance.InvokeEvent(mEventKey);
                    m_TriggeredThisContact = true;
                }
            //}
        }

        // return directly if it can only be used once
        if(m_TriggeredThisContact && !isReusable)
        {
            return;
        }

        if (!m_UpdateThisFrame)
        {
            m_Contacted = false;
            m_TriggeredThisContact = false;
            // m_SpawnPointChange = false;
        }

        // Reset it to false at the end for use in the next frame
        m_UpdateThisFrame = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == mContactObject)
        {
            Debug.Log("Trigger stay");
            SetContacted();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == mContactObject)
        {
            Debug.Log("Collision stay");
            SetContacted();
        }
    }

    // All these can work because OnTrigger and OnCollision happens before Update()
    public void SetContacted()
    {
        m_Contacted = true;
        m_UpdateThisFrame = true;
    }

    public string GetEventKey()
    {
        return mEventKey;
    }

    public GameObject GetContactObject()
    {
        return mContactObject;
    }

    public bool GetEventTrigger()
    {
        return isEventTrigger;
    }

    public bool IsSpawnpointTrigger()
    {
        return isSpawnPointTrigger;
    }

    /*
    public void SetSpawnPointTriggered()
    {
        m_SpawnPointChange = true;
    }

    public bool GetSpawnPointTriggered()
    {
        return m_SpawnPointChange;
    }
    */
}


