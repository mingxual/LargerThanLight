using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public List<GameObject> m_AllObjects;
    [SerializeField] List<Wall3D> m_AllWalls;
    [SerializeField] List<Obstacle> m_AllObstacles;
    [SerializeField] List<Mesh> m_AllMeshes;
    [SerializeField] List<List<Vector3>> m_AllMeshVertices;
    [SerializeField] GameObject m_RootObject;
    [SerializeField] List<GameObject> m_GameObjectPool;
    [SerializeField] List<EdgeCollider2D> m_EdgeCollider2DPool;
    [SerializeField] Transform m_SkiaSpawnTransform;

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
        m_AllWalls = new List<Wall3D>();
        m_AllObstacles = new List<Obstacle>();
        m_AllMeshes = new List<Mesh>();
        m_AllMeshVertices = new List<List<Vector3>>();

        for(int i = 0; i < m_AllObjects.Count; i++)
        {
            Wall3D wall3D = m_AllObjects[i].GetComponent<Wall3D>();
            if(wall3D)
            {
                m_AllWalls.Add(wall3D);
            }
            Obstacle obstacle = m_AllObjects[i].GetComponent<Obstacle>();
            if(obstacle)
            {
                m_AllObstacles.Add(obstacle);
            }
        }

        for(int i = 0; i < m_AllObstacles.Count; i++)
        {
            m_AllMeshes.Add(m_AllObstacles[i].GetComponent<MeshFilter>().mesh);
            int numVertices = m_AllMeshes[i].vertexCount;
            List<Vector3> vertexPositions = new List<Vector3>();
            for (int j = 0; j < numVertices; j++)
            {
                Vector3 pos = m_AllObstacles[i].transform.localToWorldMatrix * m_AllMeshes[i].vertices[j];
                if (!vertexPositions.Contains(pos))
                    vertexPositions.Add(pos);
            }

            m_AllMeshVertices.Add(vertexPositions);
        }

        //Create parent for pooled objects
        m_RootObject = new GameObject();
        m_RootObject.name = "Object Pool";
        m_RootObject.transform.parent = gameObject.transform;

        m_GameObjectPool = new List<GameObject>();
        m_EdgeCollider2DPool = new List<EdgeCollider2D>();

        PhysicsMaterial2D physicsMaterial = new PhysicsMaterial2D();
        physicsMaterial.name = "Edge collider physics material";
        physicsMaterial.friction = 0.0f;
        physicsMaterial.bounciness = 0.0f;

        //Create pool of gameobjects and store edge colliders
        for (int i = 0; i < m_AllObstacles.Count; ++i)
        {
            GameObject gameObject = new GameObject();
            gameObject.layer = 10;
            gameObject.transform.SetParent(m_RootObject.transform);
            gameObject.transform.position = m_RootObject.transform.position;
            gameObject.AddComponent<ShadowMoveSkia>();
            gameObject.tag = "Shadow";
            EdgeCollider2D edgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();
            edgeCollider2D.sharedMaterial = physicsMaterial;
            m_EdgeCollider2DPool.Add(edgeCollider2D);

            if (m_AllObjects[i].GetComponent<EventCollision2D>() != null)
            {
                gameObject.AddComponent<EventCollision2D>();

                EventCollision2D originEventCollision2D = m_AllObjects[i].GetComponent<EventCollision2D>();
                EventCollision2D copiedEventCollision2D = gameObject.GetComponent<EventCollision2D>();

                copiedEventCollision2D.m_EventKey = originEventCollision2D.m_EventKey;
                copiedEventCollision2D.m_TriggerObject = originEventCollision2D.m_TriggerObject;
                copiedEventCollision2D.m_TriggerOnlyOnce = originEventCollision2D.m_TriggerOnlyOnce;
            }
            else if (m_AllObjects[i].GetComponent<EventTrigger2D>() != null)
            {
                gameObject.GetComponent<EdgeCollider2D>().isTrigger = true;
                gameObject.AddComponent<EventTrigger2D>();

                EventTrigger2D originEventCollision2D = m_AllObjects[i].GetComponent<EventTrigger2D>();
                EventTrigger2D copiedEventCollision2D = gameObject.GetComponent<EventTrigger2D>();

                copiedEventCollision2D.m_EventKey = originEventCollision2D.m_EventKey;
                copiedEventCollision2D.m_TriggerObject = originEventCollision2D.m_TriggerObject;
                copiedEventCollision2D.m_TriggerOnlyOnce = originEventCollision2D.m_TriggerOnlyOnce;
            }

            gameObject.SetActive(false);
            m_GameObjectPool.Add(gameObject);
        }
    }

    public List<Obstacle> GetCurrentObstacles()
    {
        return m_AllObstacles;
    }

    public List<Mesh> GetCurrentMeshes()
    {
        return m_AllMeshes;
    }

    public List<List<Vector3>> GetCurrentMeshVertices()
    {
        return m_AllMeshVertices;
    }

    public List<EdgeCollider2D> GetCurrentEdgeColliderPool()
    {
        return m_EdgeCollider2DPool;
    }

    public List<GameObject> GetCurrentGameObjectPool()
    {
        return m_GameObjectPool;
    }

    public Transform GetSkiaSpawnTransform()
    {
        return m_SkiaSpawnTransform;
    }
}
