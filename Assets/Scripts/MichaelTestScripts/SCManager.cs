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
    public LayerMask m_Wall2DLayerMask;
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

        worldPositions = new List<Vector3>();
    }

    public SimpleController GetSkia()
    {
        return skia;
    }

    private void FixedUpdate()
    {
        RaycastLights();
    }

    List<ShadowEntityProjectile> shadowProjs = new List<ShadowEntityProjectile>();
    List<Vector3> worldPositions;


    RaycastHit[] obstacles = new RaycastHit[50];
    public void RaycastLights()
    {
        worldPositions.Clear();
        if(skia.gameObject.activeSelf)
            worldPositions.Add(skia.GetWorldPosition());
        foreach(ShadowEntityProjectile shadowProj in shadowProjs)
        {
            worldPositions.Add(shadowProj.GetWorldPosition());
        }

        float skiaLightPosition = -1;

        int lightCasts = 0;
        //print("light count: " + LevelManager.Instance.GetCurrentSegment().GetLights().Count);

        int poolI = 0;
        for(int k = 0; k < worldPositions.Count; k++)
        {
            Vector3 worldPosition = worldPositions[k];

            foreach (SCLight sclight in LevelManager.Instance.GetCurrentSegment().GetLights())
            //foreach (SCLight sclight in FindObjectsOfType<SCLight>())
            {
                if (!sclight.active) continue;
                Light light = sclight.GetComponent<Light>();
                Vector3 lightPos = light.transform.position;
                /*Debug.DrawLine(worldPosition, lightPos, Color.white);
                Debug.Log("lux forward: " + light.transform.forward);
                Debug.Log("spot angle: " + light.spotAngle);
                float angle = Vector3.Angle(worldPosition - light.transform.position, light.transform.forward);
                Debug.Log("angle: " + angle);*/

                float angle = Vector3.Angle(worldPosition - lightPos, light.transform.forward);
                if (angle > light.spotAngle / 2 || Vector3.Magnitude(worldPosition - lightPos) > light.range)
                    continue;
                else if(k == 0)
                {
                    if (angle > light.innerSpotAngle / 2)
                    {
                        float value = (angle - light.innerSpotAngle / 2) / (light.spotAngle / 2 - light.innerSpotAngle / 2);
                        skiaLightPosition = skiaLightPosition == -1 ? value : Mathf.Min(skiaLightPosition, value);
                    }
                    else
                    {
                        skiaLightPosition = 0;
                    }
                }

                int obstacleCount = Physics.SphereCastNonAlloc(new Ray(lightPos, worldPosition - lightPos), lightcast_radius, obstacles, Vector3.Magnitude(worldPosition - lightPos), m_ObstacleLayerMask, QueryTriggerInteraction.Collide);
                //print("obs count: " + obstacles.Length);
                for (int i = 0; i < obstacleCount; i++)
                {
                    RaycastHit obstacle = obstacles[i];
                    if (obstacle.transform.GetComponent<SCObstacle>().shadowprojaffected) continue;
                    //if (Vector3.Angle(obstacle.transform.position - lightPos, light.transform.forward) > light.spotAngle / 2 || Vector3.Magnitude(obstacle.transform.position - lightPos) > light.range)
                    //{
                    //    print("wew " + obstacle.transform.name);
                    //    //continue;
                    //}

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

                    //print("num vertices: " + numVertices);

                    Vector3 currObstaclePos = obstacle.transform.position;
                    m_CurrProjectedPoints2D.Clear();
                    for (int j = 0; j < numVertices; ++j)
                    {
                        Vector3 p = obstacle.transform.localToWorldMatrix * vertices[j];
                        RaycastHit hitInfo;
                        Vector3 dir = currObstaclePos + p - lightPos;
                        dir = dir.normalized;
                        if (Physics.Raycast(lightPos, dir, out hitInfo, 200.0f, m_WallLayerMask, QueryTriggerInteraction.Collide))
                        {
                            Wall3D wall3D = hitInfo.collider.GetComponent<Wall3D>();
                            Vector3 point = wall3D.RaycastToWall2D(hitInfo.transform.InverseTransformPoint(hitInfo.point), transform.position, obstacle.collider.GetComponent<SCObstacle>().debugLines);
                            Vector2 point2D = Vector2.right * point.x + Vector2.up * point.y;
                            m_CurrProjectedPoints2D.Add(point2D + wall3D.coordinate2D);

                            if (m_ShowObjectRaycasts || obstacle.collider.GetComponent<SCObstacle>().debugLines)
                            {
                                Debug.DrawRay(lightPos, dir * hitInfo.distance, Color.blue);
                                Debug.DrawRay(hitInfo.point, point - hitInfo.point, Color.green);
                                //print("Casted vertex " + p + " to point " + hitInfo.point + " transformed to " + point2D);
                            }
                            lightCasts++;
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
                        ShadowMoveSkia shadowMoveSkia = poolGO.AddComponent<ShadowMoveSkia>();
                        shadowMoveSkia.Initialize();
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
                        handle.SetSpawnpointTrigger(false);
                    }
                    else if (originEventTrigger2D)
                    {
                        poolGO.GetComponent<PolygonCollider2D>().isTrigger = true;

                        if (originEventTrigger2D.GetEnableStatus())
                        {
                            handle.SetEventKey(originEventTrigger2D.GetEventKey());
                            handle.SetContactObject(originEventTrigger2D.GetContactObject());
                            handle.isEventTrigger = true;
                            handle.isEventCollider = false;
                            handle.SetSpawnpointTrigger(originEventTrigger2D.IsSpawnpointTrigger());
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
                            handle.SetSpawnpointTrigger(false);
                        }
                    }
                }
            }

            if(k == 0)
            {
                skia.SetLightStatus(skiaLightPosition);
            }
        }

        while (poolI < m_ObstaclePool.Count)
        {
            m_ObstaclePool[poolI].SetActive(false);
            poolI++;
        }

        //print(lightCasts);
    }

    public bool RaycastSpawnpoint(out Vector2 position)
    {
        Transform spawnpoint = LevelManager.Instance.GetSkiaSpawnpoint();
        //print("spawnpoint at " + spawnpoint.position);
        if(!spawnpoint)
        {
            position = Vector2.zero;
            //print("no spawnpoint");
            return false;
        }

        List<Vector2> points = new List<Vector2>();
        foreach (SCLight sclight in LevelManager.Instance.GetCurrentSegment().GetLights())
        {
            if (!sclight.active) continue;
            Light light = sclight.GetComponent<Light>();
            Vector3 lightPos = light.transform.position;
            if (Vector3.Angle(spawnpoint.position - lightPos, light.transform.forward) > light.spotAngle / 2 || Vector3.Magnitude(spawnpoint.position - lightPos) > light.range)
                continue;
            RaycastHit hitInfo;
            Vector3 dir = spawnpoint.position - lightPos;
            dir = dir.normalized;
            if (Physics.Raycast(lightPos, dir, out hitInfo, 10000.0f, m_WallLayerMask, QueryTriggerInteraction.Collide))
            {
                Wall3D wall3D = hitInfo.collider.GetComponent<Wall3D>();
                Vector3 point = wall3D.RaycastToWall2D(hitInfo.transform.InverseTransformPoint(hitInfo.point), transform.position);
                //print(sclight.name + " " + hitInfo.point);
                points.Add(point);
            }
        }

        if(points.Count == 0)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(spawnpoint.position, spawnpoint.forward, out hitInfo, 200.0f, m_WallLayerMask, QueryTriggerInteraction.Collide))
            {
                Wall3D wall3D = hitInfo.collider.GetComponent<Wall3D>();
                Vector3 point = wall3D.RaycastToWall2D(hitInfo.transform.InverseTransformPoint(hitInfo.point), transform.position);
                position = point;
                //print("spawnpoint hit wall at " + position);
                return false;
            }
            position = Vector2.zero;
            //print("spawnpoint invalid");
            return false;
        }
        else if(points.Count == 1)
        {
            position = points[0];
            //print("spawnpoint in light at " + position);
            return true;
        }
        else
        {
            Vector2 sum = Vector2.zero;
            foreach(Vector2 point in points)
            {
                sum += point;
            }
            position = sum / points.Count;
            //print("spawnpoint in " + points.Count + " lights. center at " + position);
            return true;
        }
    }

    public void AddShadowProj(ShadowEntityProjectile proj)
    {
        if (shadowProjs.Contains(proj)) return;
        shadowProjs.Add(proj);
    }

    public void RemoveShadowProj(ShadowEntityProjectile proj)
    {
        if(shadowProjs.Contains(proj))
        {
            shadowProjs.Remove(proj);
        }
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

        } while (points[p] != points[l]); // While we don't come to first  
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
            wallGO.name = wall.name + " 2D";
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
        upperPad2DGO.name = "Upper Padding " + wall2D.name;
        upperPad2DGO.transform.position = wall2D.transform.position + wall2D.transform.localScale.y * Vector3.up;
        upperPad2DGO.transform.rotation = wall2D.transform.rotation;
        upperPad2DGO.transform.localScale = wall2D.transform.localScale;
        upperPad2DGO.AddComponent<BoxCollider>();
        upperPad2DGO.transform.SetParent(wall2D.transform.parent);

        GameObject upperPad3DGO = new GameObject();
        upperPad3DGO.name = "Upper Padding " + wall3D.name;
        upperPad3DGO.transform.position = wall3D.transform.position + wall3D.transform.localScale.y * Vector3.up;
        upperPad3DGO.transform.rotation = wall3D.transform.rotation;
        upperPad3DGO.transform.localScale = wall3D.transform.localScale;
        upperPad3DGO.AddComponent<BoxCollider>();
        upperPad3DGO.layer = LayerMask.NameToLayer("Wall");
        upperPad3DGO.transform.SetParent(wall2D.transform.parent);

        upperPad2DGO.AddComponent<Wall2D>().wall3D = upperPad3DGO;
        upperPad3DGO.AddComponent<Wall3D>().wall2D = upperPad2DGO;

        GameObject lowerPad2DGO = new GameObject();
        lowerPad2DGO.name = "Lower Padding " + wall2D.name;
        lowerPad2DGO.transform.position = wall2D.transform.position - wall2D.transform.localScale.y * Vector3.up;
        lowerPad2DGO.transform.rotation = wall2D.transform.rotation;
        lowerPad2DGO.transform.localScale = wall2D.transform.localScale;
        lowerPad2DGO.AddComponent<BoxCollider>();
        lowerPad2DGO.transform.SetParent(wall2D.transform.parent);

        GameObject lowerPad3DGO = new GameObject();
        lowerPad3DGO.name = "Lower Padding " + wall3D.name;
        lowerPad3DGO.transform.position = wall3D.transform.position - wall3D.transform.localScale.y * Vector3.up;
        lowerPad3DGO.transform.rotation = wall3D.transform.rotation;
        lowerPad3DGO.transform.localScale = wall3D.transform.localScale;
        lowerPad3DGO.AddComponent<BoxCollider>();
        lowerPad3DGO.layer = LayerMask.NameToLayer("Wall");
        lowerPad3DGO.transform.SetParent(wall2D.transform.parent);

        lowerPad2DGO.AddComponent<Wall2D>().wall3D = lowerPad3DGO;
        lowerPad3DGO.AddComponent<Wall3D>().wall2D = lowerPad2DGO;
    }

    private void BuildOuterPadding()
    {
        Wall3D wall3D = wallOrdering[0];
        Wall2D wall2D = wall3D.wall2D.GetComponent<Wall2D>();

        GameObject pad2DGO = new GameObject();
        pad2DGO.name = "Left Padding 2D";
        pad2DGO.transform.position = wall2D.transform.position - wall2D.transform.localScale.x * wall2D.transform.right;
        pad2DGO.transform.rotation = wall2D.transform.rotation;
        pad2DGO.transform.localScale = new Vector3(wall2D.transform.localScale.x, wall2D.transform.localScale.y * 3, wall2D.transform.localScale.z);
        pad2DGO.AddComponent<BoxCollider>();
        pad2DGO.transform.SetParent(wall2D.transform.parent);

        GameObject pad3DGO = new GameObject();
        pad3DGO.name = "Left Padding 3D";
        pad3DGO.transform.position = wall3D.transform.position - wall3D.transform.localScale.x * wall3D.transform.right;
        pad3DGO.transform.rotation = wall3D.transform.rotation;
        pad3DGO.transform.localScale = new Vector3(wall3D.transform.localScale.x, wall3D.transform.localScale.y * 3, wall3D.transform.localScale.z);
        pad3DGO.AddComponent<BoxCollider>();
        pad3DGO.layer = LayerMask.NameToLayer("Wall");
        pad3DGO.transform.SetParent(wall2D.transform.parent);

        pad2DGO.AddComponent<Wall2D>().wall3D = pad3DGO;
        pad3DGO.AddComponent<Wall3D>().wall2D = pad2DGO;

        wall3D = wallOrdering[wallOrdering.Count - 1];
        wall2D = wall3D.wall2D.GetComponent<Wall2D>();

        pad2DGO = new GameObject();
        pad2DGO.name = "Right Padding 2D";
        pad2DGO.transform.position = wall2D.transform.position + wall2D.transform.localScale.x * wall2D.transform.right;
        pad2DGO.transform.rotation = wall2D.transform.rotation;
        pad2DGO.transform.localScale = new Vector3(wall2D.transform.localScale.x, wall2D.transform.localScale.y * 3, wall2D.transform.localScale.z);
        pad2DGO.AddComponent<BoxCollider>();
        pad2DGO.transform.SetParent(wall2D.transform.parent);

        pad3DGO = new GameObject();
        pad3DGO.name = "Right Padding 3D";
        pad3DGO.transform.position = wall3D.transform.position + wall3D.transform.localScale.x * wall3D.transform.right;
        pad3DGO.transform.rotation = wall3D.transform.rotation;
        pad3DGO.transform.localScale = new Vector3(wall3D.transform.localScale.x, wall3D.transform.localScale.y * 3, wall3D.transform.localScale.z);
        pad3DGO.AddComponent<BoxCollider>();
        pad3DGO.layer = LayerMask.NameToLayer("Wall");
        pad3DGO.transform.SetParent(wall2D.transform.parent);

        pad2DGO.AddComponent<Wall2D>().wall3D = pad3DGO;
        pad3DGO.AddComponent<Wall3D>().wall2D = pad2DGO;
    }
}