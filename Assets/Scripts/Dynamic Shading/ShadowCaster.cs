using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCaster : MonoBehaviour
{
    public GameObject[] objectToCastShadow;
    public LayerMask wall;
    Mesh[] mesh;
    List<Vector2> projectedPoints;
    public bool showWireframe;
    public bool showRayToProjectedPoint;
    public bool showProjectedWireframe;
    int numObstacles;

    private void Start()
    {
        objectToCastShadow = GameObject.FindGameObjectsWithTag("Obstacle"); //Get all objects tagged as obstacle
        mesh = new Mesh[objectToCastShadow.Length]; //Initialize mesh array to store all objects' meshes
        if (objectToCastShadow.Length > 0) //If at least one object exists in game
        {
            numObstacles = objectToCastShadow.Length; //Get obstacle count

            //Store all objects' meshes
            for (int i = 0; i < numObstacles; ++i)
            {
                mesh[i] = objectToCastShadow[i].GetComponent<MeshFilter>().mesh;
            }
        }
        else
        {
            Debug.LogError("Error in ShadowCaster.Start(): No objects were found. Make sure objects are tagged as 'obstacle' in editor.");
        }

        //Initialize
        projectedPoints = new List<Vector2>();
    }

    private void Update()
    {
        for (int i = 0; i < numObstacles; ++i) //Loop through all obstacles
        {
            //Clear projected points from last object
            if (projectedPoints.Count > 0)
            {
                projectedPoints.Clear();
            }

            Project(i);

            // GenerateBoundingVolume(i);
        }
    }

    void Project(int index)
    {
        int numVertices = mesh[index].vertexCount; //Get number of vertices from mesh of current object indicated by its index

        for (int i = 0; i < numVertices; ++i) //Loop through all vertices
        {
            //Positions with respect to gameobject's center
            Vector3 p = mesh[index].vertices[i]; 

            //Shoot rays through current vertex to project it onto the wall
            //Store projected point
            RaycastHit hitInfo;
            Vector3 dir = objectToCastShadow[index].transform.position + p - transform.position; //Vector from light to object's vertex
            dir = dir.normalized;
            Ray ray = new Ray(transform.position, dir); //Origin: light's position, Direction: light position to vertex on object
            if (Physics.Raycast(ray, out hitInfo, 100.0f, wall, QueryTriggerInteraction.Collide))
            {
                if(showRayToProjectedPoint) //Show rays from light to projected point on the wall
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * hitInfo.distance, Color.cyan);

                //Store hit points
                projectedPoints.Add(hitInfo.point + Vector3.forward * 0.01f); //The Vector3.forward * 0.01 is to offset the points, because the player z-fights the wall...
            }
        }
    }

    void GenerateBoundingVolume(int index)
    {
        List<Vector2> boundedVolumePoints = ConvexHull(projectedPoints);
        if (boundedVolumePoints == null)
        {
            Debug.LogError("No results were generated from convex hull in ShadowCaster.CreateColliders()");
            return;
        }

        if(showProjectedWireframe) //Show wireframe of projected points on wall based from convex hull algorithm
        {
            for (int i = 0; i < boundedVolumePoints.Count; ++i)
            {
                if(i == boundedVolumePoints.Count - 1)
                {
                    Debug.DrawLine(boundedVolumePoints[i], boundedVolumePoints[0], Color.green);
                }
                else
                {
                    Debug.DrawLine(boundedVolumePoints[i], boundedVolumePoints[i + 1], Color.green);
                }
            }
        }
    }

    List<Vector2> ConvexHull(List<Vector2> points)
    {
        //Null if not enough points
        if (points.Count < 3) return null;

        //Initialize
        List<Vector2> hull = new List<Vector2>();

        //Find leftmost point
        int l = 0;
        for(int i = 1; i < points.Count; ++i)
        {
            if(points[i].x < points[l].x)
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

    void OnDrawGizmos()
    {
        if (showWireframe) //Show wireframe of obstacles if enabled
        {
            for(int k = 0; k < numObstacles; ++k)//Loop through all obstacles
            {
                int numIndices = mesh[k].triangles.Length;

                for (int i = 0; i < numIndices; i += 3)//Loop through mesh triangles of each object
                {
                    //Draw lines that make up each triangle
                    Vector3 p1 = objectToCastShadow[k].transform.position + mesh[k].vertices[mesh[k].triangles[i]];
                    Vector3 p2 = objectToCastShadow[k].transform.position + mesh[k].vertices[mesh[k].triangles[i + 1]];
                    Vector3 p3 = objectToCastShadow[k].transform.position + mesh[k].vertices[mesh[k].triangles[i + 2]];
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(p1, p2);
                    Gizmos.DrawLine(p2, p3);
                    Gizmos.DrawLine(p3, p1);
                    Gizmos.color = Color.blue;
                }
            }
        }
    }
}
