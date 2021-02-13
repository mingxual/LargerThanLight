using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCManager : MonoBehaviour
{
    public static SCManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    [Tooltip("Order of walls to be projected on from left to right. Note: walls must all be the same height.")]
    [SerializeField] List<Wall3D> wallOrdering;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] float m_CoordX = 0.0f, m_CoordY = -500.0f;
    GameObject wallsTransformParent;

    [SerializeField] SimpleController skia;
    [SerializeField] float lightcast_radius = 4;

    public LayerMask m_WallLayerMask;
    public LayerMask m_ObstacleLayerMask;
    public bool m_ShowObjectRaycasts = false;

    //Computing points at runtime
    List<Vector2> m_CurrProjectedPoints2D;
    List<Vector2> m_CurrConvexedPoints2D;
    List<Vector2> m_ConvexHullPoints;

    List<GameObject> m_ObstaclePool;

    PhysicsMaterial2D m_physicsMaterial;

    private void Start()
    {
        m_CurrProjectedPoints2D = new List<Vector2>();
        m_CurrConvexedPoints2D = new List<Vector2>();
        m_ConvexHullPoints = new List<Vector2>();

        m_ObstaclePool = new List<GameObject>();

        m_physicsMaterial = new PhysicsMaterial2D("Edge collider physics material");
        m_physicsMaterial.friction = 0;
        m_physicsMaterial.bounciness = 0;
    }

    private void FixedUpdate()
    {
        RaycastLights();
    }

    public void RaycastLights()
    {
        int lightCasts = 0;
        print("light count: " + LevelManager.Instance.GetCurrentSegment().GetLights().Count);


        int poolI = 0;
        foreach (SCLight sclight in LevelManager.Instance.GetCurrentSegment().GetLights())
        {
            if (!sclight.active) continue;
            Light light = sclight.GetComponent<Light>();
            Vector3 lightPos = light.transform.position;
            //Debug.DrawLine(skia.GetWorldPosition(), lightPos, Color.white);
            if (Vector3.Angle(skia.GetWorldPosition() - light.transform.position, light.transform.forward) > light.spotAngle / 2 || Vector3.Magnitude(skia.GetWorldPosition() - light.transform.position) > light.range)
                continue;
            RaycastHit[] obstacles = Physics.SphereCastAll(new Ray(lightPos, skia.GetWorldPosition() - lightPos), lightcast_radius, Vector3.Magnitude(skia.GetWorldPosition() - lightPos) + 1, m_ObstacleLayerMask, QueryTriggerInteraction.Collide);
            print("obs count: " + obstacles.Length);
            for (int i = 0; i < obstacles.Length; i++)
            {
                RaycastHit obstacle = obstacles[i];
                int numVertices = 0;
                Vector3[] vertices = new Vector3[0];
                if (obstacle.collider is MeshCollider)
                {
                    MeshCollider obstacleM = (MeshCollider)obstacle.collider;
                    numVertices = obstacleM.sharedMesh.vertexCount;
                    vertices = obstacleM.sharedMesh.vertices;
                }
                else if (obstacle.collider is BoxCollider)
                {
                    BoxCollider obstacleB = (BoxCollider)obstacle.collider;
                    numVertices = 8;
                    vertices = GetVerticesFromBox(obstacleB);
                }
                else
                {
                    continue;
                }

                print("num vertices: " + numVertices);

                Vector3 currObstaclePos = obstacle.transform.position;
                m_CurrProjectedPoints2D.Clear();
                for (int j = 0; j < numVertices; ++j)
                {
                    Vector3 p = obstacle.collider.transform.localToWorldMatrix * vertices[j];
                    RaycastHit hitInfo;
                    Vector3 dir = currObstaclePos + p - lightPos;
                    dir = dir.normalized;
                    if (Physics.Raycast(lightPos, dir, out hitInfo, 10000.0f, m_WallLayerMask, QueryTriggerInteraction.Collide))
                    {
                        Wall3D wall3D = hitInfo.collider.gameObject.GetComponent<Wall3D>();
                        Vector3 point = wall3D.RaycastToWall2D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point), transform.position);
                        Vector2 point2D = Vector2.right * point.x + Vector2.up * point.y;
                        m_CurrProjectedPoints2D.Add(point2D + wall3D.coordinate2D);
                        if (m_ShowObjectRaycasts)
                        {
                            Debug.DrawRay(lightPos, dir * hitInfo.distance, Color.blue);
                            Debug.DrawRay(hitInfo.point, point - hitInfo.point, Color.green);
                            lightCasts++;
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

                GameObject poolGO;
                if (poolI < m_ObstaclePool.Count)
                {
                    poolGO = m_ObstaclePool[poolI];
                    poolI++;
                }
                else
                {
                    poolGO = new GameObject();
                    poolGO.transform.SetParent(transform);
                    poolGO.transform.position = transform.position;
                    poolGO.layer = 10;
                    poolGO.AddComponent<PolygonCollider2D>().sharedMaterial = m_physicsMaterial;
                    poolGO.AddComponent<SCEventHandle>();
                    poolGO.AddComponent<ShadowMoveSkia>();
                    poolGO.tag = "Shadow";

                    m_ObstaclePool.Add(poolGO);
                }

                poolGO.SetActive(true);
                poolGO.GetComponent<PolygonCollider2D>().points = convexedPoints.ToArray();
                SCEventHandle handle = poolGO.GetComponent<SCEventHandle>();
                handle.corrObject = obstacle.transform.gameObject;


                // Check if there is any script attached
                EventTrigger2D originEventTrigger2D = obstacle.transform.GetComponent<EventTrigger2D>();
                EventCollision2D originEventCollision2D = obstacle.transform.GetComponent<EventCollision2D>();
                if (originEventTrigger2D == null && originEventCollision2D == null)
                {
                    handle.isEventTrigger = false;
                    handle.isEventCollider = false;
                }
                else if(originEventTrigger2D)
                {
                    poolGO.GetComponent<PolygonCollider2D>().isTrigger = true;

                    if (originEventTrigger2D.GetEnableStatus())
                    {
                        handle.SetEventKey(originEventTrigger2D.GetEventKey());
                        handle.SetContactObject(originEventTrigger2D.GetContactObject());
                        handle.isEventTrigger = true;
                        handle.isEventCollider = false;
                    }
                }
                else
                {
                    if (originEventCollision2D.GetEnableStatus())
                    {
                        handle.SetEventKey(originEventCollision2D.GetEventKey());
                        handle.SetContactObject(originEventCollision2D.GetContactObject());
                        handle.isEventTrigger = false;
                        handle.isEventCollider = true;
                    }
                }
            }
        }

        while (poolI < m_ObstaclePool.Count)
        {
            m_ObstaclePool[poolI].SetActive(false);
            poolI++;
        }

        print(lightCasts);
    }

    Vector3[] GetVerticesFromBox(BoxCollider box)
    {
        Vector3[] vertices = new Vector3[8];
        Vector3 center = box.center, size = box.size;
        vertices[0] = new Vector3(center.x + size.x / 2, center.y + size.y / 2, center.z + size.z / 2);
        vertices[1] = new Vector3(center.x + size.x / 2, center.y + size.y / 2, center.z - size.z / 2);
        vertices[2] = new Vector3(center.x + size.x / 2, center.y - size.y / 2, center.z + size.z / 2);
        vertices[3] = new Vector3(center.x + size.x / 2, center.y - size.y / 2, center.z - size.z / 2);
        vertices[4] = new Vector3(center.x - size.x / 2, center.y + size.y / 2, center.z + size.z / 2);
        vertices[5] = new Vector3(center.x - size.x / 2, center.y + size.y / 2, center.z - size.z / 2);
        vertices[6] = new Vector3(center.x - size.x / 2, center.y - size.y / 2, center.z + size.z / 2);
        vertices[7] = new Vector3(center.x - size.x / 2, center.y - size.y / 2, center.z - size.z / 2);
        return vertices;
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

    int Orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        float val = (q.y - p.y) * (r.x - q.x) - (q.x - p.x) * (r.y - q.y);
        if (val == 0.0f) return 0; //Collinear
        return (val > 0.0f) ? 1 : 2; //Clockwise or counter-clockwise
    }

    public void BuildWalls()
    {
        if (!wallPrefab || wallOrdering.Count == 0) return;

        float wallHeight = -1;
        foreach (Wall3D wall in wallOrdering)
        {
            if (wallHeight == -1)
            {
                wallHeight = wall.transform.localScale.y;
            }
            else if (wallHeight != wall.transform.localScale.y)
            {
                Debug.LogError("Walls are not all the same height. " + wallHeight + " != " + wall.transform.localScale.y);
                return;
            }
        }

        wallsTransformParent = new GameObject();
        wallsTransformParent.name = "Walls";
        wallsTransformParent.transform.position = new Vector3(m_CoordX, m_CoordY, 0.0f);
        wallsTransformParent.AddComponent<EdgeCollider2D>();

        float wallXPos = 0;
        foreach (Wall3D wall in wallOrdering)
        {
            GameObject wallGO = Instantiate(wallPrefab, wallsTransformParent.transform);
            wallGO.SetActive(true);
            wallGO.transform.localScale = new Vector3(wall.transform.localScale.x, wallHeight, 0.01f);
            wallGO.transform.localPosition = new Vector2(wallXPos + wall.transform.localScale.x / 2, wallHeight / 2);
            wallGO.GetComponentInChildren<Camera>().targetTexture = (RenderTexture)wall.GetComponent<Renderer>().sharedMaterial.mainTexture;
            Wall2D wall2D = wallGO.GetComponent<Wall2D>();
            wall2D.wall3D = wall.gameObject;
            wall.wall2D = wall2D.gameObject;
            wall.gameObject.layer = LayerMask.NameToLayer("Wall");

            BuildPadding(wall2D, wall);

            wallXPos += wall.transform.localScale.x;
        }

        BuildOuterPadding();

        EdgeCollider2D wallEdgeCollider = wallsTransformParent.GetComponent<EdgeCollider2D>();
        wallEdgeCollider.points = new Vector2[5] { Vector2.zero, new Vector2(wallXPos, 0), new Vector2(wallXPos, wallHeight), new Vector2(0, wallHeight), Vector2.zero };
        wallsTransformParent.layer = 10;
    }

    private void BuildPadding(Wall2D wall2D, Wall3D wall3D)
    {
        GameObject upperPad2DGO = new GameObject();
        upperPad2DGO.transform.position = wall2D.transform.position + wall2D.transform.localScale.y * Vector3.up;
        upperPad2DGO.transform.rotation = wall2D.transform.rotation;
        upperPad2DGO.transform.localScale = wall2D.transform.localScale;
        upperPad2DGO.AddComponent<BoxCollider>();
        upperPad2DGO.transform.SetParent(wall2D.transform);

        GameObject upperPad3DGO = new GameObject();
        upperPad3DGO.transform.position = wall3D.transform.position + wall3D.transform.localScale.y * Vector3.up;
        upperPad3DGO.transform.rotation = wall3D.transform.rotation;
        upperPad3DGO.transform.localScale = wall3D.transform.localScale;
        upperPad3DGO.AddComponent<BoxCollider>();
        upperPad3DGO.layer = LayerMask.NameToLayer("Wall");
        upperPad3DGO.transform.SetParent(wall2D.transform);

        upperPad2DGO.AddComponent<Wall2D>().wall3D = upperPad3DGO;
        upperPad3DGO.AddComponent<Wall3D>().wall2D = upperPad2DGO;

        GameObject lowerPad2DGO = new GameObject();
        lowerPad2DGO.transform.position = wall2D.transform.position - wall2D.transform.localScale.y * Vector3.up;
        lowerPad2DGO.transform.rotation = wall2D.transform.rotation;
        lowerPad2DGO.transform.localScale = wall2D.transform.localScale;
        lowerPad2DGO.AddComponent<BoxCollider>();
        lowerPad2DGO.transform.SetParent(wall2D.transform);

        GameObject lowerPad3DGO = new GameObject();
        lowerPad3DGO.transform.position = wall3D.transform.position - wall3D.transform.localScale.y * Vector3.up;
        lowerPad3DGO.transform.rotation = wall3D.transform.rotation;
        lowerPad3DGO.transform.localScale = wall3D.transform.localScale;
        lowerPad3DGO.AddComponent<BoxCollider>();
        lowerPad3DGO.layer = LayerMask.NameToLayer("Wall");
        lowerPad3DGO.transform.SetParent(wall2D.transform);

        lowerPad2DGO.AddComponent<Wall2D>().wall3D = lowerPad3DGO;
        lowerPad3DGO.AddComponent<Wall3D>().wall2D = lowerPad2DGO;
    }

    private void BuildOuterPadding()
    {
        Wall3D wall3D = wallOrdering[0];
        Wall2D wall2D = wall3D.wall2D.GetComponent<Wall2D>();

        GameObject pad2DGO = new GameObject();
        pad2DGO.transform.position = wall2D.transform.position - wall2D.transform.localScale.x * wall2D.transform.right;
        pad2DGO.transform.rotation = wall2D.transform.rotation;
        pad2DGO.transform.localScale = new Vector3(wall2D.transform.localScale.x, wall2D.transform.localScale.y * 3, wall2D.transform.localScale.z);
        pad2DGO.AddComponent<BoxCollider>();
        pad2DGO.transform.SetParent(wall2D.transform);

        GameObject pad3DGO = new GameObject();
        pad3DGO.transform.position = wall3D.transform.position - wall3D.transform.localScale.x * wall3D.transform.right;
        pad3DGO.transform.rotation = wall3D.transform.rotation;
        pad3DGO.transform.localScale = new Vector3(wall3D.transform.localScale.x, wall3D.transform.localScale.y * 3, wall3D.transform.localScale.z);
        pad3DGO.AddComponent<BoxCollider>();
        pad3DGO.layer = LayerMask.NameToLayer("Wall");
        pad3DGO.transform.SetParent(wall2D.transform);

        pad2DGO.AddComponent<Wall2D>().wall3D = pad3DGO;
        pad3DGO.AddComponent<Wall3D>().wall2D = pad2DGO;

        wall3D = wallOrdering[wallOrdering.Count - 1];
        wall2D = wall3D.wall2D.GetComponent<Wall2D>();

        pad2DGO = new GameObject();
        pad2DGO.transform.position = wall2D.transform.position + wall2D.transform.localScale.x * wall2D.transform.right;
        pad2DGO.transform.rotation = wall2D.transform.rotation;
        pad2DGO.transform.localScale = new Vector3(wall2D.transform.localScale.x, wall2D.transform.localScale.y * 3, wall2D.transform.localScale.z);
        pad2DGO.AddComponent<BoxCollider>();
        pad2DGO.transform.SetParent(wall2D.transform);

        pad3DGO = new GameObject();
        pad3DGO.transform.position = wall3D.transform.position + wall3D.transform.localScale.x * wall3D.transform.right;
        pad3DGO.transform.rotation = wall3D.transform.rotation;
        pad3DGO.transform.localScale = new Vector3(wall3D.transform.localScale.x, wall3D.transform.localScale.y * 3, wall3D.transform.localScale.z);
        pad3DGO.AddComponent<BoxCollider>();
        pad3DGO.layer = LayerMask.NameToLayer("Wall");
        pad3DGO.transform.SetParent(wall2D.transform);

        pad2DGO.AddComponent<Wall2D>().wall3D = pad3DGO;
        pad3DGO.AddComponent<Wall3D>().wall2D = pad2DGO;
    }
}