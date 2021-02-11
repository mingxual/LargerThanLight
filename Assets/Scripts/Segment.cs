using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    [SerializeField] List<SCLight> m_AllLights;
    [SerializeField] GameObject m_WallParent;
    [SerializeField] Camera m_RenderCam;
    [SerializeField] Transform m_SkiaSpawnTransform;
    [SerializeField] Transform m_LuxSpawnTransform;
    [SerializeField] SCLight m_LuxLight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        //Do nothing
    }

    public void Activate()
    {
        if (m_WallParent)
            m_WallParent.SetActive(true);

        for(int i = 0; i < m_AllLights.Count; i++)
        {
            m_AllLights[i].active = true;
        }

        if(m_RenderCam)
            m_RenderCam.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        if (m_WallParent)
            m_WallParent.SetActive(false);

        for (int i = 0; i < m_AllLights.Count; i++)
        {
            m_AllLights[i].active = false;
        }

        if (m_RenderCam)
            m_RenderCam.gameObject.SetActive(false);
    }

    public Transform GetSkiaSpawnTransform()
    {
        return m_SkiaSpawnTransform;
    }

    public Transform GetLuxSpawnTransform()
    {
        return m_LuxSpawnTransform;
    }

    public List<SCLight> GetLights()
    {
        return m_AllLights;
    }

    public SCLight GetLuxLight()
    {
        return m_LuxLight;
    }
}
