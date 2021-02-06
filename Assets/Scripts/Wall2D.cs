using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall2D : MonoBehaviour
{
    public GameObject wall3D;

    public Vector3 SwitchTo3D(Vector3 point)
    {
        return wall3D.transform.TransformPoint(point);
    }
}
