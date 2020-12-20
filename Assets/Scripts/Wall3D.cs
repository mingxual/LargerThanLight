using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall3D : MonoBehaviour
{
    /*[System.Serializable]
    public struct Coordinate2D
    {
        public float x, y;
        public Coordinate2D(float _x, float _y)
        {
            x = _x;
            y = _y;
        }
    }*/

    // The corresponding 2D wall in the scene
    public GameObject wall2D;

    //public Coordinate2D coordinate2D;

    // The offset to the origin of 2D walls in the scene
    public Vector2 coordinate2D;
    public Material m_OriginalMaterial;
    MeshRenderer m_MeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Create gameobjects for wall2D
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 RaycastToWall2D(Vector3 point, Vector3 originPos)
    {
        Debug.DrawRay(originPos, wall2D.transform.TransformPoint(point) - originPos, Color.red);
        return wall2D.transform.TransformPoint(point);
    }

    public Vector3 SwitchTo2D(Vector3 point)
    {
        return wall2D.transform.TransformPoint(point - wall2D.transform.forward * 1.0f);
    }

    public GameObject GetWall2D()
    {
        if (wall2D)
        {
            return wall2D;
        }

        return null;
    }

    public void OriginalMaterial()
    {
        if(m_OriginalMaterial)
            m_MeshRenderer.material = m_OriginalMaterial;

        transform.gameObject.layer = 0;
    }
}
