using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCLight : MonoBehaviour
{
    public bool active;

    Light m_Light; //This gameobject's light component
    //float m_LightOuterAngle;
    //float m_LightRange;

    private void Start()
    {
        //Get light info
        m_Light = GetComponent<Light>();
        //m_LightOuterAngle = m_Light.spotAngle / 2.0f;
        //m_LightRange = m_Light.range;
    }

    private void FixedUpdate()
    {
        if (active)
            SCManager.Instance.RaycastLight(m_Light);
    }
}
