using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WallTeleport : MonoBehaviour
{
    [SerializeField] GameObject wall;

    public void ChangeWalls()
    {
        if (!wall) return;

        if(wall.GetComponent<Wall2D>())
        {
            Quaternion tempRot = transform.rotation * Quaternion.Inverse(wall.transform.rotation);
            transform.position = wall.GetComponent<Wall2D>().SwitchTo3D(wall.transform.InverseTransformPoint(transform.position));
            wall = wall.GetComponent<Wall2D>().wall3D;
            transform.rotation = tempRot * wall.transform.rotation;
        }
        else if(wall.GetComponent<Wall3D>())
        {
            Quaternion tempRot = transform.rotation * Quaternion.Inverse(wall.transform.rotation);
            transform.position = wall.GetComponent<Wall3D>().SwitchTo2D(wall.transform.InverseTransformPoint(transform.position));
            wall = wall.GetComponent<Wall3D>().wall2D;
            transform.rotation = tempRot * wall.transform.rotation;
        }
    }
}
