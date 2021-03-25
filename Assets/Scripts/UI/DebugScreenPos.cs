using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScreenPos : MonoBehaviour
{
    public SimpleController skia;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 screenPos = cam.WorldToScreenPoint(skia.GetWorldPosition3D());
        Debug.Log("Skia's screen position is " + screenPos.x + " " + screenPos.y + " " + screenPos.z);
    }
}
