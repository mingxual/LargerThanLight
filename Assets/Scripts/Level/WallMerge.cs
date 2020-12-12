using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMerge : MonoBehaviour
{
    Collider[] m_Colliders;

    // Start is called before the first frame update
    void Start()
    {
        //Get all colliders
        m_Colliders = GetComponents<Collider>();

        //Find closest wall
        Wall3D closestWall = LevelManager.m_AllSubLevels[0].m_AllWalls[0];
        Vector3 closestPositionToWall = closestWall.transform.position - transform.position;
        float closestDistToWall = closestPositionToWall.sqrMagnitude;
        for (int i = 0; i < LevelManager.m_AllSubLevels.Count; i++)
        {
            for(int j = 0; j < LevelManager.m_AllSubLevels[i].m_AllWalls.Length; j++)
            {
                Vector3 posToWall = LevelManager.m_AllSubLevels[i].m_AllWalls[j].transform.position - transform.position;
                float distToWall = posToWall.sqrMagnitude;
                if(distToWall < closestDistToWall)
                {
                    closestDistToWall = distToWall;
                    closestWall = LevelManager.m_AllSubLevels[i].m_AllWalls[j];
                }
            }
        }

        //Merge this object into closest wall
        for(int i = 0; i < m_Colliders.Length; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
