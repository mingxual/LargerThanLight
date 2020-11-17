using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCollision2D : MonoBehaviour
{
    // The string of the key (match to the one declared in EventsManager)
    public string m_EventKey;

    // The gameobject that would trigger the event
    public GameObject m_TriggerObject;

    // The current event can be triggered once or unlimited times
    public bool m_TriggerOnlyOnce;

    private bool m_HasTriggered;
    private bool m_IsTriggering;

    private float m_Timer;
    public bool isCollided;

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
            // Fungus.Flowchart.BroadcastFungusMessage("Curtain Pulled");
            EventsManager.instance.InvokeEvent(m_EventKey);

            //audio added
            // AudioManager.instance.PlayOnce("Curtain_Open", new Vector3(0, 0, 0));
        }

        if(Time.time - m_Timer > 0.5f)
        {
            isCollided = false;
        }
    }

    
    public void OnCollisionStay2D(Collision2D collision)
    {
        if(m_TriggerObject != null)
        {
            if(m_TriggerObject == collision.gameObject && !m_IsTriggering)
            {
                m_Timer = Time.time;
                isCollided = true;
                /*
                Fungus.Flowchart.BroadcastFungusMessage("Curtain Pulled");

                //audio added
                AudioManager.instance.PlayOnce("Curtain_Open", new Vector3(0, 0, 0));
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
                */
            }
        }
        else
        {
            EventsManager.instance.InvokeEvent(m_EventKey);
            m_IsTriggering = true;
        }
    }
}
