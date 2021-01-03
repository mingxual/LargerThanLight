using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform skiaTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currPos = transform.position;
        Vector3 skiaPos = skiaTransform.position;
        currPos.x = skiaPos.x;
        transform.position = currPos;
    }
}
