using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : MonoBehaviour
{
    public float m_Width;
    public float m_Height;
    float m_Ratio;
    Camera m_Camera;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<Camera>();
        m_Ratio = m_Width / m_Height;
        m_Camera.aspect = m_Ratio;
    }

    // Update is called once per frame
    void Update()
    {
        m_Ratio = m_Width / m_Height;
        m_Camera.aspect = m_Ratio;
        Debug.Log("camera aspect: " + m_Camera.aspect);
        /*Rect rect = m_Camera.rect;
        rect.width = m_Width;
        rect.height = m_Height;
        m_Camera.rect = rect;*/
    }
}
