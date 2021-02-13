using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollision2D : MonoBehaviour
{
    // The string of the key (match to the one declared in EventsManager)
    [SerializeField] string m_EventKey;

    // The gameobject that would trigger the event (would use the collider of the gameobject to trigger)
    [SerializeField] GameObject m_ContactObject;

    // Variables to track whether the script is invoked or not
    private bool m_Triggered;
    private bool m_Collided;

    // This is the variable to track whether the current script is on or off
    public bool m_OnEnable = true;

    // Start is called before the first frame update
    void Start()
    {
        m_Triggered = false;
        m_Collided = false;
        // m_OnEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Collided && !m_Triggered && Input.GetAxis("Interaction") > 0.8f)
        {
            EventsManager.instance.InvokeEvent(m_EventKey);
            m_Triggered = true;
        }

        // currently disable this ui stuff, will pick up later to come up with a better solution
        // dealing with UI hints
        /*
        if (isCollided && !m_IsTriggering && m_EventKey.Contains("UI"))
        {
            m_IsTriggering = true;
            EventsManager.instance.InvokeEvent(m_EventKey);
        }
        */
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == m_ContactObject)
        {
            EventsManager.instance.InvokeEvent(m_EventKey);
            m_Collided = true;
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
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

    public void SetEventKey(string eventKey)
    {
        m_EventKey = eventKey;
    }

    public string GetEventKey()
    {
        return m_EventKey;
    }

    public void SetContactObject(GameObject contactObject)
    {
        m_ContactObject = contactObject;
    }

    public GameObject GetContactObject()
    {
        return m_ContactObject;
    }
}
