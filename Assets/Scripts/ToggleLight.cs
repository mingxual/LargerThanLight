using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    public KeyCode toggleKey;
    private Light m_Light;
    private bool isChanged;

    // Start is called before the first frame update
    void Start()
    {
        m_Light = GetComponent<Light>();
        isChanged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(toggleKey) && !isChanged)
        {
            m_Light.enabled = !m_Light.enabled;
            isChanged = true;
        }
        else if(Input.GetKeyUp(toggleKey))
        {
            isChanged = false;
        }
    }
}
