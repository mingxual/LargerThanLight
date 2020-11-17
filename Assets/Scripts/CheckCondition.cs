using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCondition : MonoBehaviour
{
    public string m_EventKey;
    public bool isValid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckCon()
    {
        Debug.Log("enter here");
        if (isValid)
        {
            // Debug.Log("here");
            EventsManager.instance.InvokeEvent(m_EventKey);
        }
    }

    public void SetCon(bool val)
    {
        isValid = val;
    }
}
