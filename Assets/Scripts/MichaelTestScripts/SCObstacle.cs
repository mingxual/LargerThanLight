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

    // If the obstacle can be affected by shadow projectiles;
    public bool shadowprojaffect;
    // How long the obstacle is affected by shadow projectiles;
    public float shadowprojtime;

    // Whether the obstacle is currently affected by shadow projectiles;
    public bool shadowprojaffected;

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

    public void HitByShadowProj()
    {
        if (!shadowprojaffect) return;
        shadowprojaffected = true;
        ToggleRenderersForShadowProj(false);
        Invoke("RecoverFromShadowProj", shadowprojtime);
    }

    private void RecoverFromShadowProj()
    {
        ToggleRenderersForShadowProj(true);
        shadowprojaffected = false;
    }

    private void ToggleRenderersForShadowProj(bool flag)
    {
        MeshRenderer[] meshRenderers = GetComponents<MeshRenderer>();
        if (meshRenderers.Length == 0)
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }

        foreach(MeshRenderer mr in meshRenderers)
        {
            mr.shadowCastingMode = (UnityEngine.Rendering.ShadowCastingMode)(flag ? 1 : 0);
        }
    }
}
