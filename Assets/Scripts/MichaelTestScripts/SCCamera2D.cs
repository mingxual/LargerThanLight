using UnityEngine;

[ExecuteInEditMode]
public class SCCamera2D : MonoBehaviour
{
    float m_Width, m_Height, m_Ratio;
    Camera m_Camera;

    private void Start()
    {
        m_Camera = GetComponent<Camera>();
    }

    private void Update()
    {
        m_Width = transform.parent.localScale.x;
        m_Height = transform.parent.localScale.y;
        m_Ratio = m_Width / m_Height;
        m_Camera.aspect = m_Ratio;
        m_Camera.orthographicSize = m_Height / 2;
    }
}
