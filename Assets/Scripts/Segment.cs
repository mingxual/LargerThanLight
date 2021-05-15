using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    [SerializeField] List<SCLight> m_AllLights;
    [SerializeField] GameObject m_WallParent;
    [SerializeField] GameObject m_ObstacleParent;
    [SerializeField] GameObject m_SpawnpointParent;
    [SerializeField] Camera m_RenderCam;
    //[SerializeField] Transform m_SkiaSpawnTransform;
    //[SerializeField] Transform m_LuxSpawnTransform;

    [SerializeField] List<Transform> m_SkiaSpawnTransforms;
    [SerializeField] List<Transform> m_LuxSpawnTransforms;
    [SerializeField] List<Collider> m_CollidersToToggleOnOff;

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
        if (m_ObstacleParent)
            m_ObstacleParent.SetActive(true);
        if (m_SpawnpointParent)
            m_SpawnpointParent.SetActive(true);

        for (int i = 0; i < m_AllLights.Count; i++)
        {
            m_AllLights[i].active = true;
        }

        if(m_RenderCam)
            m_RenderCam.gameObject.SetActive(true);

        for(int i = 0; i < m_CollidersToToggleOnOff.Count; i++)
        {
            m_CollidersToToggleOnOff[i].enabled = true;
        }
    }

    public void Deactivate()
    {
        if (m_WallParent)
            m_WallParent.SetActive(false);
        if (m_ObstacleParent)
            m_ObstacleParent.SetActive(false);
        if (m_SpawnpointParent)
            m_SpawnpointParent.SetActive(false);

        for (int i = 0; i < m_AllLights.Count; i++)
        {
            m_AllLights[i].active = false;
        }

        if (m_RenderCam)
            m_RenderCam.gameObject.SetActive(false);

        for (int i = 0; i < m_CollidersToToggleOnOff.Count; i++)
        {
            m_CollidersToToggleOnOff[i].enabled = false;
        }
    }

    public Transform GetSkiaFirstSpawnTransform()
    {
        //return m_SkiaSpawnTransform;
        return m_SkiaSpawnTransforms[0];
    }

    public Transform GetLuxFirstSpawnTransform()
    {
        return m_LuxSpawnTransforms[0];
    }

    public Transform GetLuxSpawnTransform(Transform skia)
    {
        int skiaSpawnpointIndex = m_SkiaSpawnTransforms.IndexOf(skia);
        if(skiaSpawnpointIndex < m_LuxSpawnTransforms.Count)
            return m_LuxSpawnTransforms[skiaSpawnpointIndex];
        return null;
    }

    public List<SCLight> GetLights()
    {
        return m_AllLights;
    }

    public bool HasSkiaSpawnpoint(Transform t)
    {
        return m_SkiaSpawnTransforms.Contains(t);
    }
}
