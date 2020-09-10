using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger2D : MonoBehaviour
{
    // The string of the key (match to the one declared in EventsManager)
    public string m_EventKey;

    // The gameobject that would trigger the event
    public GameObject m_TriggerObject;

    // The current event can be triggered once or unlimited times
    public bool m_TriggerOnlyOnce;

    private bool m_HasTriggered;
    private bool m_IsTriggering;

    // Start is called before the first frame update
    void Start()
    {
        m_HasTriggered = false;
        m_IsTriggering = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(m_TriggerObject != null)
        {
            if(m_TriggerObject == collision.gameObject && !m_IsTriggering)
            {
                if(m_TriggerOnlyOnce && !m_HasTriggered)
                {
                    EventsManager.instance.InvokeEvent(m_EventKey);
                    m_HasTriggered = true;
                }
                else if(!m_TriggerOnlyOnce)
                {
                    EventsManager.instance.InvokeEvent(m_EventKey);
                }
                m_IsTriggering = true;
            }
        }
        else
        {
            EventsManager.instance.InvokeEvent(m_EventKey);
            m_IsTriggering = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (m_TriggerObject != null)
        {
            if(m_TriggerObject == collision.gameObject)
            {
                m_IsTriggering = false;
            }
        }
        else
        {
            m_IsTriggering = false;
        }
    }
}
