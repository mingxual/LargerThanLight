using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCObstacle : MonoBehaviour
{
    public bool enableRotation;
    public Vector3 rotationAxis;
    public float rotationSpeed;

    // Occlusion variables
    bool m_IsOccluded;
    float m_FadeTime = 0.4f;
    float m_CurrFadeTime;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
        m_CurrFadeTime = 0f;
        m_IsOccluded = true;
        m_FadeTime = .4f;
    }

    private void Update()
    {
        if(enableRotation)
        {
            if (rotationAxis.sqrMagnitude > 0)
                transform.Rotate(rotationAxis, Time.deltaTime * rotationSpeed);
        }
    }
}
