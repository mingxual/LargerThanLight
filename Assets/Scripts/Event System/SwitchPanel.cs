using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPanel : MonoBehaviour
{
    public List<ToggleLight> spotLights;
    private bool isCollided;
    private bool isTriggered;

    // Start is called before the first frame update
    void Start()
    {
        isCollided = false;
        isTriggered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Interaction") > 0.8f && isCollided && !isTriggered)
        {
            isTriggered = true;
            for(int i = 0; i < spotLights.Count; ++i)
            {
                spotLights[i].SwitchOnAndOff();
            }
        }
        else if(Input.GetAxis("Interaction") < 0.1f)
        {
            isTriggered = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Lux")
        {
            isCollided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Lux")
        {
            isCollided = false;
        }
    }
}
