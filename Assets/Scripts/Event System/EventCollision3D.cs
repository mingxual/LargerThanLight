using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCollision3D : MonoBehaviour
{
    // The string of the key (match to the one declared in EventsManager)
    [SerializeField] string m_EventKey;

    // The gameobject that would trigger the event (would use the collider of the gameobject to trigger)
    [SerializeField] GameObject m_ContactObject;

    // true when need to press space to invoke, false when not
    [SerializeField] bool isSpaceInputNeeded = true;

    // Variables to track whether the script is invoked or not
    private bool m_Triggered;
    private bool m_Collided;

    // This is the variable to track whether the current script is on or off
    private bool m_OnEnable;

    // Start is called before the first frame update
    void Start()
    {
        m_Triggered = false;
        m_Collided = false;
        m_OnEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Collided && !m_Triggered && Input.GetAxis("Interaction") > 0.8f)
        {
            EventsManager.instance.InvokeEvent(m_EventKey);
            m_Triggered = true;
        }

        if (m_Collided && !m_Triggered && !isSpaceInputNeeded)
        {
            EventsManager.instance.InvokeEvent(m_EventKey);
            m_Triggered = true;
            return;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.name);
        if (collision.gameObject == m_ContactObject)
        {
            m_Collided = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == m_ContactObject)
        {
            m_Collided = false;
        }
    }

    public void SetEnableStatus(bool val)
    {
        m_OnEnable = val;
    }

    public bool GetEnableStatus()
    {
        return m_OnEnable;
    }
}
