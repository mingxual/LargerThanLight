using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatedSCEventHandler : MonoBehaviour
{
    [SerializeField] string m_EventKey;
    [SerializeField] GameObject m_ContactObject;
    [SerializeField] bool m_IsSpawnpoint;

    // true when trigger, false when collide
    public bool isEventTrigger;

    // Variable to track the mapping 3D object that the shadow belongs to
    public GameObject corrObject;


    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == m_ContactObject)
        {
            if (m_IsSpawnpoint)
            {
                //ShadowEventHandler corrHandler = corrObject.GetComponent<ShadowEventHandler>();
                //if (!corrHandler.GetSpawnPointTriggered())
                //{
                    LevelManager.Instance.SetSkiaSpawnpoint(corrObject.transform);
                //    corrHandler.SetSpawnPointTriggered();
                //}
            }
            else
            {
                Debug.Log("Trigger stay");
                corrObject.GetComponent<ShadowEventHandler>().SetContacted();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (m_ContactObject && collision.gameObject == m_ContactObject)
        {
            Debug.Log(m_EventKey);
            ShadowEventHandler corrHandler = corrObject.GetComponent<ShadowEventHandler>();
            if(corrHandler) corrHandler.SetContacted();
        }
    }

    public void SetEventKey(string eventKey)
    {
        m_EventKey = eventKey;
    }

    public void SetContactObject(GameObject contactObject)
    {
        m_ContactObject = contactObject;
    }

    public void SetSpawnpointTrigger(bool flag)
    {
        m_IsSpawnpoint = flag;
    }
}
