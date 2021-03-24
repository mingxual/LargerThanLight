using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextSeg : MonoBehaviour
{
    public string mEventKey;

    // Update is called once per frame
    void Update()
    {
        
    }
    void StartNextSeg()
    {
        EventsManager.instance.InvokeEvent(mEventKey);
        Destroy(gameObject);
    }
}
