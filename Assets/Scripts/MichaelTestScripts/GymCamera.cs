using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymCamera : MonoBehaviour
{
    [SerializeField] SimpleController skia;

    public float largeRotation;
    public Vector2 zRange;

    public float zRSpeed;
    public float zSpeed;

    Vector3 forward = new Vector3(-1, 0, 0);

    private void Update()
    {
        Vector3 skiaPos = skia.GetWorldPosition();
        if(skiaPos.z < zRange.x)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, zRange.x), zSpeed * Time.deltaTime);

            Vector3 skiaDir = skiaPos - transform.position;
            skiaDir.y = 0;
            skiaDir.Normalize();
            float dotp = Vector3.Dot(skiaDir, forward);
            float theta = Mathf.Acos(dotp) * 180 / Mathf.PI;
            print(theta);
            if (theta > largeRotation)
                theta = largeRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, -90 - theta, transform.rotation.eulerAngles.z), zRSpeed * Time.deltaTime);
        }
        else if(skiaPos.z > zRange.y)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, zRange.y), zSpeed * Time.deltaTime);

            Vector3 skiaDir = skiaPos - transform.position;
            skiaDir.y = 0;
            skiaDir.Normalize();
            float dotp = Vector3.Dot(skiaDir, forward);
            float theta = Mathf.Acos(dotp) * 180 / Mathf.PI;
            if (theta > largeRotation)
                theta = largeRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, -90 + theta, transform.rotation.eulerAngles.z), zRSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y, skiaPos.z), zSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x, -90, transform.rotation.eulerAngles.z), zRSpeed * Time.deltaTime);
        }
    }
}
