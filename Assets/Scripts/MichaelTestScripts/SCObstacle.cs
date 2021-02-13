using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCObstacle : MonoBehaviour
{
    public bool enableRotation;
    public Vector3 rotationAxis;
    public float rotationSpeed;

    // Occlusion variables
    Material m_TransparentMaterial;
    bool m_IsOccluded;
    float m_FadeTime = 0.4f;
    float m_CurrFadeTime;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        m_TransparentMaterial = GetComponent<MeshRenderer>().materials[0];
        m_CurrFadeTime = 0f;
        m_IsOccluded = true;
        m_FadeTime = .4f;
    }

    private void FixedUpdate()
    {
        m_IsOccluded = false;
    }

    private void Update()
    {
        if(enableRotation)
        {
            if (rotationAxis.sqrMagnitude > 0)
                transform.Rotate(rotationAxis, Time.deltaTime * rotationSpeed);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!m_IsOccluded)
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
            if (m_CurrFadeTime < 0f)
            {
                m_CurrFadeTime = 0f;
            }
        }

        Color materialColor = m_TransparentMaterial.color;
        materialColor.a = Mathf.Lerp(.4f, 1f, (m_CurrFadeTime / m_FadeTime));
        m_TransparentMaterial.color = materialColor;
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
