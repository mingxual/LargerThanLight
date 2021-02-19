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
        GetComponent<Collider2D>().isTrigger = isEventTrigger;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == m_ContactObject)
        {
            if (m_IsSpawnpoint)
            {
                LevelManager.Instance.SetSkiaSpawnpoint(GetComponent<SCEventHandle>().corrObject.transform);
            }
            else
            {
                Debug.Log("Trigger stay");
                corrObject.GetComponent<ShadowEventHandler>().SetContacted();
            }

            if (corrObject)
            {
                corrObject.SendMessage("CollisionTrigger", collision.gameObject, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == m_ContactObject)
        {
            Debug.Log("Collision stay");
            corrObject.GetComponent<ShadowEventHandler>().SetContacted();
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
