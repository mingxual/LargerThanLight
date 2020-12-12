using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    /*public struct Level
    {
        int m_ID;
        SubLevel[] m_SubLevels;
    }*/

    // Declare singleton
    private static LevelManager _instance;
    public static LevelManager Instance
    { 
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<LevelManager>();
            }

            return _instance;
        }
    }


    [SerializeField] string m_LevelNamePrefix = "SUBLEVEL"; // Sublevel name prefix - sublevel objects will take on this prefix for processing

    [System.Serializable]
    public struct SubLevel
    {
        public GameObject[] m_AllObjects;
        public Wall3D[] m_AllWalls;
        public WallMerge[] m_AllWallMerges;
        public Obstacle[] m_AllObstacles;
        public Grid2D m_Grid2D;
        public bool m_IsActivated;
        public int m_ID;
    }

    public static List<SubLevel> m_AllSubLevels;

    private void Awake()
    {
        m_AllSubLevels = new List<SubLevel>();

        // Get all root game objects in scene
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        int sublevelCount = 1;

        GameObject padGameObject = new GameObject();
        padGameObject.name = "Padding 3D";

        // Loop through all root game objects
        for(int i = 0; i < rootObjects.Length; i++)
        {
            string sublevelName = m_LevelNamePrefix + " " + sublevelCount.ToString(); // Append level string name
            if(rootObjects[i].name.Contains(sublevelName)) // Check if sublevel name exists
            {
                SubLevel sublevel = new SubLevel(); // Create new sublevel

                // Query for sublevel information from sublevel object
                sublevel.m_AllWalls = ProcessWalls(rootObjects[i]);
                sublevel.m_AllWallMerges = ProcessWallMerges(rootObjects[i]);
                sublevel.m_AllObstacles = ProcessObstacles(rootObjects[i]);
                sublevel.m_AllObjects = ProcessObjects(sublevel);
                sublevel.m_Grid2D = new Grid2D();
                sublevel.m_ID = sublevelCount++;
                m_AllSubLevels.Add(sublevel);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Wall3D[] ProcessWalls(GameObject go)
    {
        Wall3D[] allWalls = go.transform.GetComponentsInChildren<Wall3D>();
        return allWalls;
    }

    WallMerge[] ProcessWallMerges(GameObject go)
    {
        WallMerge[] allWallMerges = go.transform.GetComponentsInChildren<WallMerge>();
        return allWallMerges;
    }

    Obstacle[] ProcessObstacles(GameObject go)
    {
        Obstacle[] allObstacles = go.transform.GetComponentsInChildren<Obstacle>();
        return allObstacles;
    }

    GameObject[] ProcessObjects(SubLevel sublevel)
    {
        List<GameObject> gameobjects = new List<GameObject>();

        for(int i = 0; i < sublevel.m_AllWalls.Length; i++)
        {
            if(!gameobjects.Contains(sublevel.m_AllWalls[i].gameObject))
            {
                gameobjects.Add(sublevel.m_AllWalls[i].gameObject);
            }
        }

        for (int i = 0; i < sublevel.m_AllWallMerges.Length; i++)
        {
            if (!gameobjects.Contains(sublevel.m_AllWallMerges[i].gameObject))
            {
                gameobjects.Add(sublevel.m_AllWallMerges[i].gameObject);
            }
        }

        for (int i = 0; i < sublevel.m_AllObstacles.Length; i++)
        {
            if (!gameobjects.Contains(sublevel.m_AllObstacles[i].gameObject))
            {
                gameobjects.Add(sublevel.m_AllObstacles[i].gameObject);
            }
        }

        return gameobjects.ToArray();
    }
}
