using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private Material m_Material;
    [SerializeField] float m_TotalFadeTime = 2.0f;
    float m_CurrFadeTime = 0.0f;
    private bool m_IsFading = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_IsFading)
        {
            m_CurrFadeTime += Time.deltaTime;
            if(m_CurrFadeTime > m_TotalFadeTime)
            {
                m_CurrFadeTime = m_TotalFadeTime;
            }
            Color color = m_Material.color;
            color.a = Mathf.Lerp(1.0f, 0.0f, (m_CurrFadeTime / m_TotalFadeTime));
            m_Material.color = color;
        }
    }

    public void FadeObject()
    {
        if (!m_Material) m_Material = GetComponent<MeshRenderer>().material;
        m_IsFading = true;
    }
}
