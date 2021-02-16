using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger2D : MonoBehaviour
{
    // The string of the key (match to the one declared in EventsManager)
    [SerializeField] string m_EventKey;

    // The gameobject that would trigger the event (would use the collider of the gameobject to trigger)
    [SerializeField] GameObject m_ContactObject;

    // If the trigger is a spawnpoint set for Skia
    [SerializeField] bool m_SpawnpointTrigger;

    // Variables to track the invoke status (such as whether it is invoked)
    private bool m_Triggered;

    // This is the variable to track whether the current script is on or off
    // For copying from 3D space to 2D space purpose
    private bool m_OnEnable;


    // Start is called before the first frame update
    void Start()
    {
        // Set the default value
        m_Triggered = false;
        m_OnEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == m_ContactObject && !m_Triggered)
        {
            EventsManager.instance.InvokeEvent(m_EventKey);
            m_Triggered = true;
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

    public bool IsSpawnpointTrigger()
    {
        return m_SpawnpointTrigger;
    }
}
