using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    /*[SerializeField] GameObject m_CopyObstacle;
    Material m_Material;*/
    bool m_IsOccluded;
    float m_FadeTime = 0.4f;

    float m_CurrFadeTime;

    // Start is called before the first frame update
    void Start()
    {
        //m_Material = m_CopyObstacle.GetComponent<MeshRenderer>().material;
        m_CurrFadeTime = 0f;
        m_IsOccluded = true;
        m_FadeTime = .4f;
    }

    private void FixedUpdate()
    {
        m_IsOccluded = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!m_IsOccluded)
        {
            m_CurrFadeTime += Time.deltaTime;
            if (m_CurrFadeTime > m_FadeTime)
            {
                m_CurrFadeTime = m_FadeTime;
            }
        }
        else
        {
            m_CurrFadeTime -= Time.deltaTime;
            if(m_CurrFadeTime < 0f)
            {
                m_CurrFadeTime = 0f;
            }
        }

        //Color materialColor = m_Material.color;
        //materialColor.a = Mathf.Lerp(.4f, 1f, (m_CurrFadeTime / m_FadeTime));
        //m_Material.color = materialColor;
    }

    public void Occlude()
    {
        m_IsOccluded = true;
    }

    public void NonOcclude()
    {
        m_IsOccluded = false;
    }
}
