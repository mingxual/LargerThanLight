using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCObstacle : MonoBehaviour
{
    public bool enableRotation;
    public Vector3 rotationAxis;
    public float rotationSpeed;

    public bool debugLines;

    // Occlusion variables
    Material[] m_TransparentMaterials;
    bool m_IsOccluded;
    float m_FadeTime = 0.4f;
    float m_CurrFadeTime;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        MeshRenderer[] meshRenderers = GetComponents<MeshRenderer>();
        if(meshRenderers.Length == 0)
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }
        m_TransparentMaterials = new Material[meshRenderers.Length];
        for(int i = 0; i < meshRenderers.Length; i++)
        {
            m_TransparentMaterials[i] = meshRenderers[i].materials[0];
        }
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

        for(int i = 0; i < m_TransparentMaterials.Length; i++)
        {
            Color materialColor = m_TransparentMaterials[i].color;
            materialColor.a = Mathf.Lerp(.4f, 1f, (m_CurrFadeTime / m_FadeTime));
            m_TransparentMaterials[i].color = materialColor;
        }
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
