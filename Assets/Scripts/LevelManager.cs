using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    [SerializeField] Segment[] m_AllSegments;
    private int m_CurrentSegmentIndex = 0;

    public GameObject skia;
    public GameObject lux;

    private Transform skiaSpawnpoint;
    private Transform luxSpawnpoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeSubLevels()
    {
        //Initialize all segments - storing mesh data, creating object pools, etc
        for(int i = 0; i < m_AllSegments.Length; i++)
        {
            m_AllSegments[i].Initialize();
        }

        //Turn on first segment
        SetActiveSegment(m_CurrentSegmentIndex);
    }

    public void SetNextSegment()
    {
        if(m_CurrentSegmentIndex < m_AllSegments.Length - 1)
        {
            m_CurrentSegmentIndex++;
            SetActiveSegment(m_CurrentSegmentIndex);
            SetSkiaSpawnpoint(GetCurrentSegment().GetSkiaFirstSpawnTransform());
        }
    }

    private void SetActiveSegment(int index)
    {
        // Turn off the rest
        for(int i = 0; i < m_AllSegments.Length; i++)
        {
            if(i != index)
            {
                m_AllSegments[i].Deactivate();
            }
        }

        m_AllSegments[index].Activate(); // Turn on sublevel
    }

    public Segment GetCurrentSegment()
    {
        //Debug.Log("current segment index number: " + m_CurrentSegmentIndex);
        return m_AllSegments[m_CurrentSegmentIndex];
    }

    /// <summary>
    /// gets skia's active spawnpoint
    /// Updated 2/15 in use
    /// </summary>
    public Transform GetSkiaSpawnpoint()
    {
        if(!skiaSpawnpoint)
        {
            skiaSpawnpoint = GetCurrentSegment().GetSkiaFirstSpawnTransform();
        }
        return skiaSpawnpoint;
    }

    /// <summary>
    /// gets skia's active spawnpoint
    /// Updated 2/15 in use
    /// </summary>
    public Transform GetLuxSpawnpoint()
    {
        if (!luxSpawnpoint)
        {
            luxSpawnpoint = GetCurrentSegment().GetLuxFirstSpawnTransform();
        }
        return luxSpawnpoint;
    }

    /// <summary>
    /// sets lux's active spawnpoint
    /// Updated 2/19 in use
    /// </summary>
    public void SetSkiaSpawnpoint(Transform t)
    {
        if(GetCurrentSegment().HasSkiaSpawnpoint(t))
        {
            skiaSpawnpoint = t;
            luxSpawnpoint = GetCurrentSegment().GetLuxSpawnTransform(t);
            print("Set sp to " + t.name);
        }
        else
        {
            print("Fail set");
        }
    }
}
