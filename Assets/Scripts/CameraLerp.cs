using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] Transform m_StartingOrientation;
    [SerializeField] Transform m_EndingOrientation;
    [SerializeField] float m_TotalLerpTime = 2.0f;
    private float m_CurrentLerpTime = 0.0f;
    private bool m_IsLerping = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsLerping)
        {
            m_CurrentLerpTime += Time.deltaTime;
            if(m_CurrentLerpTime > m_TotalLerpTime)
            {
                m_CurrentLerpTime = m_TotalLerpTime;
            }
            Vector3 pos = Vector3.Lerp(m_StartingOrientation.position, m_EndingOrientation.position, (m_CurrentLerpTime / m_TotalLerpTime));
            Quaternion rot = Quaternion.Lerp(m_StartingOrientation.rotation, m_EndingOrientation.rotation, (m_CurrentLerpTime / m_TotalLerpTime));
            transform.position = pos;
            transform.rotation = rot;
        }
    }

    public void Lerp()
    {
        m_IsLerping = true;
    }
}
