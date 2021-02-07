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

    // Variables to track the invoke status (such as whether it is invoked)
    private bool m_HasTriggered;
    private bool m_IsTriggering;
    private float m_Timer;
    public bool isCollided;

    // This is the variable to track whether the current script is on or off
    private bool onEnable = true;

    // Start is called before the first frame update
    void Start()
    {
        m_HasTriggered = false;
        m_IsTriggering = false;
        isCollided = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isCollided && !m_IsTriggering && Input.GetAxis("Interaction") > 0.8f)
        {
            m_IsTriggering = true;
            EventsManager.instance.InvokeEvent(m_EventKey);
        }

        if (Time.time - m_Timer > 0.5f)
        {
            isCollided = false;
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
        /*
        if(m_TriggerObject != null)
        {
            if(m_TriggerObject == collision.gameObject && !m_IsTriggering)
            {
                m_Timer = Time.time;
                isCollided = true;
                Debug.Log(gameObject.name);
            }
        }
        */
    }

    public void SetEnableStatus(bool val)
    {
        onEnable = val;
    }

    public bool GetEnableStatus()
    {
        return onEnable;
    }
}
