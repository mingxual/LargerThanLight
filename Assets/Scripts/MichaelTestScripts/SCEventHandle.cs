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

        if (isEventCollider)
        {
            if(m_IsCollided && !m_IsTriggering && Input.GetAxis("Interaction") > 0.8f)
            {
                m_IsTriggering = true;
                EventsManager.instance.InvokeEvent(m_EventKey);
            }

            if(Time.time - m_Timer > 0.5f)
            {
                m_IsCollided = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isEventTrigger)
        {
            if(collision.gameObject == m_ContactObject && !m_HasTriggered)
            {
                if(m_IsSpawnpoint)
                {
                    LevelManager.Instance.SetSkiaSpawnpoint(GetComponent<SCEventHandle>().corrObject.transform);
                    //print("triggered spawnpoint");
                }
                else
                {
                    EventsManager.instance.InvokeEvent(m_EventKey);
                }
                m_HasTriggered = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isEventCollider)
        {
            if (collision.gameObject == m_ContactObject && !m_HasTriggered)
            {
                /*
                m_IsCollided = true;
                m_Timer = Time.time;
                */
                EventsManager.instance.InvokeEvent(m_EventKey);
                m_HasTriggered = true;
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
