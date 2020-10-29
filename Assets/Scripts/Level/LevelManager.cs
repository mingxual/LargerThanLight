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

    public string m_LevelNamePrefix = "SUBLEVEL";

    public struct SubLevel
    {
        public GameObject[] m_AllObjects;
        public Wall3D[] m_AllWalls;
        public bool m_IsActivated;
        public int m_ID;
    }

    public List<SubLevel> m_AllSubLevels;

    private void Awake()
    {
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        int sublevelCount = 1;
        for(int i = 0; i < rootObjects.Length; i++)
        {
            if(rootObjects[i].name == (m_LevelNamePrefix + " " + sublevelCount.ToString()))
            {
                SubLevel sublevel = new SubLevel();
                
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
}
