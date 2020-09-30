using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightCollider : MonoBehaviour
{
    public LayerMask wallLayerMask;
    public int m_RaycastCount;
    Light m_Light; //This gameobject's light component
    float m_LightOuterAngle;
    float m_LightRange;
    bool m_ShowRaycasts = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Light = GetComponent<Light>();
        m_LightOuterAngle = m_Light.spotAngle / 2.0f; 
        m_LightRange = m_Light.range;
    }

    // Update is called once per frame
    void Update()
    {
        m_LightOuterAngle = m_Light.spotAngle / 2.0f;
        float radius = m_LightRange * Mathf.Tan(m_LightOuterAngle * Mathf.Deg2Rad);
        Vector3 worldForward = transform.forward * m_LightRange;
        float anglePerRay = 360.0f / m_RaycastCount;
        for(int i = 0; i < m_RaycastCount; i++)
        {
            Vector3 direction = transform.right * radius * Mathf.Cos(Mathf.Deg2Rad * anglePerRay * i) + transform.up * radius * Mathf.Sin(Mathf.Deg2Rad * anglePerRay * i) + worldForward;
            Debug.DrawRay(transform.position, direction, Color.green);
        }
    }
}
