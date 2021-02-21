using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCEventHandle : MonoBehaviour
{
    public bool isEventTrigger;
    public bool isEventCollider;

    //temp
    public GameObject corrObject;

    [SerializeField] string m_EventKey;
    [SerializeField] GameObject m_ContactObject;
    [SerializeField] bool m_IsSpawnpoint;

    private bool m_HasTriggered;
    private bool m_IsTriggering;
    private float m_Timer;
    private bool m_IsCollided;

    private bool m_OnEnable;

    private void Start()
    {
        m_HasTriggered = false;
        m_IsTriggering = false;
        m_IsCollided = false;
        m_OnEnable = true;
    }

    private void Update()
    {
        if(isEventCollider && isEventTrigger)
        {
            isEventCollider = false;
        }

        GetComponent<Collider2D>().isTrigger = isEventTrigger;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isEventTrigger)
        {
            if (collision.gameObject == m_ContactObject)
            {
                if (m_IsSpawnpoint)
                {
                    LevelManager.Instance.SetSkiaSpawnpoint(GetComponent<SCEventHandle>().corrObject.transform);
                }
                else if (!m_HasTriggered)
                {
                    EventsManager.instance.InvokeEvent(m_EventKey);
                    m_HasTriggered = true;
                }
            }
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isEventCollider)
        {
            if (collision.gameObject == m_ContactObject && !m_HasTriggered)
            {
                /*
                m_IsCollided = true;
                m_Timer = Time.time;
                
                EventsManager.instance.InvokeEvent(m_EventKey);
                m_HasTriggered = true;
            }
        }
    }
    */

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(isEventCollider)
        {
            if(collision.gameObject == m_ContactObject)
            {
                Debug.Log("Collision stay");
                corrObject.GetComponent<EventCollision2D>().SetCollided(true);
            }
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

    public void SetSpawnpointTrigger(bool flag)
    {
        m_IsSpawnpoint = flag;
    }
}
