using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The object that the camera follows
    // The current version applies only to Skia
    public Transform objToFollow;
    // The 2d wall for preference to calculate the x offset
    public Transform twoDWall;
    // The 3d wall for preference to add the x offset
    public Transform threeDWall;

    public SimpleController skia;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("The length of 3d wall is " + threeDWall.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPos = transform.position;
        Vector3 objPos = skia.GetWorldPosition();
        currPos.x = objPos.x;
        transform.position = currPos;
    }
}
