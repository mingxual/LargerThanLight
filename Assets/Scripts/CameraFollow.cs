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

    [SerializeField] float maxSpeed;
    [SerializeField] float deadZone;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("The length of 3d wall is " + threeDWall.gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 skiaPos = skia.GetWorldPosition3D();
        if(transform.position.x < skiaPos.x)
        {
            if (skiaPos.x - transform.position.x < deadZone) return;

            float diffX = Mathf.Lerp(0, skiaPos.x - transform.position.x - deadZone, Time.deltaTime);
            if (diffX > maxSpeed)
                diffX = maxSpeed;
            transform.position += Vector3.right * diffX;
        }
        else if(transform.position.x > skiaPos.x)
        {
            if (transform.position.x - skiaPos.x < deadZone) return;

            float diffX = Mathf.Lerp(0, transform.position.x - skiaPos.x - deadZone, Time.deltaTime);
            if (diffX > maxSpeed)
                diffX = maxSpeed;
            transform.position -= Vector3.right * diffX;
        }
    }
}
