using System.Collections.Generic;
using UnityEngine;

// The script is attached to the lighting source
public class DynamicShadowCollision : MonoBehaviour
{
    public LayerMask m_WallLayerMask;

    //Computing points at runtime
    List<Vector2> m_CurrProjectedPoints2D;
    List<Vector2> m_CurrConvexedPoints2D;
    List<Vector2> m_ConvexHullPoints;

    //Light info
    public int m_SpotlightRaycastCount;
    Light m_Light; //This gameobject's light component
    float m_LightOuterAngle;
    float m_LightRange;
    public bool m_ShowSpotlightRaycasts = false;
    public bool m_CreateSpotlight = false;

    //Debug
    public bool m_ShowObjectRaycasts = false;

    void Start()
    {
        m_CurrProjectedPoints2D = new List<Vector2>();
        m_CurrConvexedPoints2D = new List<Vector2>();
        m_ConvexHullPoints = new List<Vector2>();

        //Get light info
        m_Light = GetComponent<Light>();
        m_LightOuterAngle = m_Light.spotAngle / 2.0f;
        m_LightRange = m_Light.range;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!LightController.luxControlsActivated)
        {
            return;
        }

        if (!GameManager.hasResettedColliderPool)
        {
            ResetGameObjectPool();
            GameManager.hasResettedColliderPool = true;
        }

        //Create colliders for obstacles
        for (int i = 0; i < GameManager.allObstacles.Count; ++i)
        {
            //Mesh mesh = allMeshes[i];
            int numVertices = GameManager.meshVertices[i].Count;
            Vector3 currObstaclePos = GameManager.allObstacles[i].transform.position;
            m_CurrProjectedPoints2D.Clear();
            for (int j = 0; j < numVertices; ++j)
            {
                Vector3 p = GameManager.meshVertices[i][j];
                RaycastHit hitInfo;
                Vector3 dir = currObstaclePos + p - transform.position;
                dir = dir.normalized;
                if (Physics.Raycast(transform.position, dir, out hitInfo, 10000.0f, m_WallLayerMask, QueryTriggerInteraction.Collide))
                {
                    Wall3D wall3D = hitInfo.collider.gameObject.GetComponent<Wall3D>();
                    //wall3D.RaycastToWall2D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point), transform.position);
                    //Vector3 point = hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point);
                    Vector3 point = wall3D.RaycastToWall2D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point), transform.position);
                    Vector2 point2D = Vector2.right * point.x + Vector2.up * point.y;
                    //Debug.Log("point2D: " + point2D);
                    m_CurrProjectedPoints2D.Add(point2D + wall3D.coordinate2D);
                    if (m_ShowObjectRaycasts)
                    {
                        Debug.DrawRay(transform.position, dir * hitInfo.distance, Color.blue);
                    }
                }
            }

            if (m_CurrProjectedPoints2D.Count == 0)
            {
                continue;
            }

            List<Vector2> convexedPoints = ConvexHull(m_CurrProjectedPoints2D);
            // Just for the safety thing
            if (convexedPoints == null || convexedPoints.Count == 0)
            {
                continue;
            }

            m_CurrConvexedPoints2D.Clear();
            int index;
            GameObject go = GetPooledGameObject(out index);
            // If valid, then implement the following
            if (go != null)
            {
                //go.layer = GameManager.gameObjectPool[i].layer;
                for (int j = 0; j < convexedPoints.Count; ++j)
                {
                    /*convexedPoints2D.Clear();
                    int index;
                    GameObject go = GetPooledGameObject(out index);*/
                    if (j == convexedPoints.Count - 1)
                    {
                        m_CurrConvexedPoints2D.Add(convexedPoints[j]);
                        m_CurrConvexedPoints2D.Add(convexedPoints[0]);
                    }
                    else
                    {
                        m_CurrConvexedPoints2D.Add(convexedPoints[j]);
                        //m_CurrConvexedPoints2D.Add(convexedPoints[j + 1]);
                    }
                    //go.transform.rotation = wall2D.transform.rotation; //Probably not necessary
                }
                go.SetActive(true);
                GameManager.edgeCollider2DPool[index].points = m_CurrConvexedPoints2D.ToArray();
            }
        }

        if(m_CreateSpotlight)
        {
            //Create colliders for spotlight
            m_CurrProjectedPoints2D.Clear();
            m_LightOuterAngle = m_Light.spotAngle / 2.0f;
            float radius = m_LightRange * Mathf.Tan(m_LightOuterAngle * Mathf.Deg2Rad);
            Vector3 worldForward = transform.forward * m_LightRange;
            float anglePerRay = 360.0f / m_SpotlightRaycastCount;
            for (int i = 0; i < m_SpotlightRaycastCount; i++)
            {
                Vector3 dir = transform.right * radius * Mathf.Cos(Mathf.Deg2Rad * anglePerRay * i) + transform.up * radius * Mathf.Sin(Mathf.Deg2Rad * anglePerRay * i) + worldForward;
                if (m_ShowSpotlightRaycasts)
                    Debug.DrawRay(transform.position, dir, Color.green);

                dir = dir.normalized;
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position, dir, out hitInfo, 10000.0f, m_WallLayerMask, QueryTriggerInteraction.Collide))
                {
                    Wall3D wall3D = hitInfo.collider.gameObject.GetComponent<Wall3D>();
                    //wall3D.RaycastToWall2D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point), transform.position);
                    //Vector3 point = hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point);
                    Vector3 point = wall3D.RaycastToWall2D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point), transform.position);
                    Vector2 point2D = Vector2.right * point.x + Vector2.up * point.y;
                    //Debug.Log("point2D: " + point2D);
                    m_CurrProjectedPoints2D.Add(point2D + wall3D.coordinate2D);
                    if (m_ShowSpotlightRaycasts)
                        Debug.DrawRay(transform.position, dir * hitInfo.distance, Color.green);
                }
            }

            if (m_CurrProjectedPoints2D.Count > 0)
            {
                List<Vector2> convexedPoints = ConvexHull(m_CurrProjectedPoints2D);
                if (convexedPoints != null)
                {
                    m_CurrConvexedPoints2D.Clear();
                    int index;
                    GameObject go = GetPooledGameObject(out index);
                    // If valid, then implement the following
                    if (go != null)
                    {
                        //go.layer = GameManager.gameObjectPool[i].layer;
                        for (int j = 0; j < convexedPoints.Count; ++j)
                        {
                            /*convexedPoints2D.Clear();
                            int index;
                            GameObject go = GetPooledGameObject(out index);*/
                            if (j == convexedPoints.Count - 1)
                            {
                                m_CurrConvexedPoints2D.Add(convexedPoints[j]);
                                m_CurrConvexedPoints2D.Add(convexedPoints[0]);
                            }
                            else
                            {
                                m_CurrConvexedPoints2D.Add(convexedPoints[j]);
                                m_CurrConvexedPoints2D.Add(convexedPoints[j + 1]);
                            }
                            //go.transform.rotation = wall2D.transform.rotation; //Probably not necessary
                        }
                        go.SetActive(true);
                        GameManager.edgeCollider2DPool[index].points = m_CurrConvexedPoints2D.ToArray();
                    }
                }
            }
        }
    }

    GameObject GetPooledGameObject(out int index)
    {

        for (int i = 0; i < GameManager.gameObjectPool.Count; ++i)
        {
            if (!GameManager.gameObjectPool[i].activeInHierarchy)
            {
                index = i;
                return GameManager.gameObjectPool[i];
            }
        }
        index = -1;
        return null;
    }

    void ResetGameObjectPool()
    {
        for(int i = 0; i < GameManager.gameObjectPool.Count; ++i)
        {
            GameManager.gameObjectPool[i].SetActive(false);
        }
    }

    List<Vector2> ConvexHull(List<Vector2> points)
    {
        //Null if not enough points
        if (points.Count < 1) return null;

        //Initialize
        m_ConvexHullPoints.Clear();

        //Find leftmost point
        int l = 0;
        for (int i = 1; i < points.Count; ++i)
        {
            if (points[i].x < points[l].x)
            {
                l = i;
            }
        }

        // Start from leftmost point, keep moving  
        // counterclockwise until reach the start point 
        // again. This loop runs O(h) times where h is 
        // number of points in result or output. 
        int p = l, q;
        do
        {
            //Add current point to result
            m_ConvexHullPoints.Add(points[p]);

            // Search for a point 'q' such that  
            // orientation(p, x, q) is counterclockwise  
            // for all points 'x'. The idea is to keep  
            // track of last visited most counterclock- 
            // wise point in q. If any point 'i' is more  
            // counterclock-wise than q, then update q.
            q = (p + 1) % points.Count;

            // If i is more counterclockwise than  
            // current q, then update q 
            for (int i = 0; i < points.Count; ++i)
            {
                if (Orientation(points[p], points[i], points[q]) == 2)
                {
                    q = i;
                }
            }

            // Now q is the most counterclockwise with 
            // respect to p. Set p as q for next iteration,  
            // so that q is added to result 'hull'
            p = q;

        } while (p != l); // While we don't come to first  
                          // point 

        return m_ConvexHullPoints;
    }

    //Returns orientation of triplet (p, q, r)
    int Orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        float val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
        if (val == 0.0f) return 0; //Collinear
        return (val > 0.0f) ? 1 : 2; //Clockwise or counter-clockwise
    }
}
