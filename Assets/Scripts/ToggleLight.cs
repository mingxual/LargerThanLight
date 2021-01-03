using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    private Light m_Light;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchOnAndOff()
    {
        m_Light.enabled = !m_Light.enabled;
    }
}
