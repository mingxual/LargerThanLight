using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayLights : MonoBehaviour
{
    public List<GameObject> spotLights;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOffLights()
    {
        for(int i = 0; i < spotLights.Count; ++i)
        {
            spotLights[i].SetActive(false);
        }
    }

    public void TurnOnLights()
    {
        for (int i = 0; i < spotLights.Count; ++i)
        {
            spotLights[i].SetActive(true);
        }
    }
}
