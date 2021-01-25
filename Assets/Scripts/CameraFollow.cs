using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPos = transform.position;
        Vector3 objPos = objTransform.position;
        currPos.x = objPos.x;
        transform.position = currPos;
    }

    public void ChangeObjTransform(Transform item)
    {
        objTransform = item;
    }
}
