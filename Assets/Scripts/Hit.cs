using System.Collections.Generic;
using UnityEngine;

// The script is attached to the lighting source
public class Hit : MonoBehaviour
{
    public LayerMask wallLayerMask;

    //Computing points at runtime
    List<Vector2> currProjectedPoints2D;
    List<Vector2> currConvexedPoints2D;

    //Debug
    public bool showRaycasts = false;

    void Start()
    {
        currProjectedPoints2D = new List<Vector2>();
        currConvexedPoints2D = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {

        ResetGameObjectPool();

        for(int i = 0; i < GameManager.allObstacles.Length; ++i)
        {
            //Mesh mesh = allMeshes[i];
            int numVertices = GameManager.meshVertices[i].Count;
            Vector3 currObstaclePos = GameManager.allObstacles[i].transform.position;
            currProjectedPoints2D.Clear();
            for(int j = 0; j < numVertices; ++j)
            {
                Vector3 p = GameManager.meshVertices[i][j];
                RaycastHit hitInfo;
                Vector3 dir = currObstaclePos + p - transform.position;
                dir = dir.normalized;
                if (Physics.Raycast(transform.position, dir, out hitInfo, 1000.0f, wallLayerMask, QueryTriggerInteraction.Collide))
                {
                    Wall3D wall3D = hitInfo.collider.gameObject.GetComponent<Wall3D>();
                    wall3D.RaycastToWall2D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point), transform.position);
                    Vector3 point = hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point);
                    Vector2 point2D = Vector2.right * point.x + Vector2.up * point.z;
                    currProjectedPoints2D.Add(point2D + wall3D.coordinate2D);
                    if (showRaycasts)
                    {
                        Debug.DrawRay(transform.position, dir * hitInfo.distance, Color.blue);
                    }
                }
            }

            if (currProjectedPoints2D.Count == 0)
            {
                continue;
            }

            List<Vector2> convexedPoints = ConvexHull(currProjectedPoints2D);
            // Just for the safety thing
            if (convexedPoints == null || convexedPoints.Count == 0)
            {
                continue;
            }

            currConvexedPoints2D.Clear();
            int index;
            GameObject go = GetPooledGameObject(out index);
            // If valid, then implement the following
            if (go != null)
            {
                for (int j = 0; j < convexedPoints.Count; ++j)
                {
                    /*convexedPoints2D.Clear();
                    int index;
                    GameObject go = GetPooledGameObject(out index);*/
                    if (j == convexedPoints.Count - 1)
                    {
                        currConvexedPoints2D.Add(convexedPoints[j]);
                        currConvexedPoints2D.Add(convexedPoints[0]);
                    }
                    else
                    {
                        currConvexedPoints2D.Add(convexedPoints[j]);
                        currConvexedPoints2D.Add(convexedPoints[j + 1]);
                    }
                    //go.transform.rotation = wall2D.transform.rotation; //Probably not necessary
                }
                go.SetActive(true);
                GameManager.edgeCollider2DPool[index].points = currConvexedPoints2D.ToArray();
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
        List<Vector2> hull = new List<Vector2>();

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
            hull.Add(points[p]);

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

        return hull;
    }

    //Returns orientation of triplet (p, q, r)
    int Orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        float val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
        if (val == 0.0f) return 0; //Collinear
        return (val > 0.0f) ? 1 : 2; //Clockwise or counter-clockwise
    }
}
