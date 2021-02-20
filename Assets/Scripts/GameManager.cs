using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Obstacle and its mesh/vertices
    /*public static List<GameObject> allObstacles;
    public static List<Mesh> allMeshes;
    public static List<List<Vector3>> meshVertices;*/

    //Pool of N game objects
    //public int poolCount = 10;
    /*public static List<GameObject> gameObjectPool;
    public static List<EdgeCollider2D> edgeCollider2DPool; //Keep a reference to pooled game object's edge collider*/
    public static bool hasResettedColliderPool = false;

    //GameObject gameObjectParent;

    //temp stuff for playable
    //TODO: clean this up; add proper organization
    /*public GameObject[] m_SubLevel1Obstacles;
    public GameObject[] m_SubLevel2Obstacles;*/
    //public GameObject[] m_SubLevel3Obstacles;
    //public GameObject originWallSubLevel1;
    /*public GameObject m_SubLevel1Object;
    public Wall3D m_SubLevel1Wall3D;
    public GameObject m_SubLevel2Object;
    public Transform SkiaSpawnSubLevel2;
    public static List<Obstacle> m_Obstacles;*/

    LevelManager m_LevelManager;

    public LightController luxControl;
    public SimpleController skiaControl;

    public GameObject lux;
    public GameObject skia;

    //Matrices Control
    public DataOutput MatricesManager;

    private void Awake()
    {
        m_LevelManager = FindObjectOfType<LevelManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_LevelManager.InitializeSubLevels();
    }

    private void Update()
    {
        // For testing
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            MatricesManager.Restart();           
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Debug
        /*if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnSkiaNextSegment();
        }*/
    }

    // Update is called once per frame
    void LateUpdate()
    {
        hasResettedColliderPool = false;
    }

    public void DisableCharacterControl()
    {
        luxControl.enabled = false;
        skiaControl.Disable();
    }

    public void EnableCharacterControl()
    {
        luxControl.enabled = true;
        skiaControl.enabled = true;
    }

    public void SpawnSkiaNextSegment()
    {
        //Debug.Break();
        m_LevelManager.SetNextSegment();

        skia.SendMessage("ResetSkia");
    }

    public void MoveLuxToPos(Transform pos)
    {
        lux.transform.position = pos.position;
    }

    public void MoveSkiaToPos(Transform pos)
    {
        skia.transform.position = pos.position;
    }

    public void SwitchToLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DisableGameObjectPool()
    {
        //gameObjectParent.SetActive(false);
    }
}
