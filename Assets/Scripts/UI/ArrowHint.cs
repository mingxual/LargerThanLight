using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowHint : MonoBehaviour
{
    public SimpleController skia;
    public LightController lux;

    float radian = 0;
    float speed = 0.1f;
    float dist = 5f;
    Vector3 oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        skia = FindObjectOfType<SimpleController>();
        lux = FindObjectOfType<LightController>();

        oldPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = skia.transform.position.z - lux.transform.position.z;
        if(distance < 0) //lux at skia's right
        {
            //oldPosition = new Vector3(880, 0, 0);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if(distance > 0) //left
        {
            //oldPosition = new Vector3(-880, 0, 0);
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        //moving animation
        radian += speed * 1000.0f / 16.0f * Time.deltaTime; //multplied 1000/16 to Time.deltaTime to not affect original value of speed
        float dx = Mathf.Cos(radian) * dist;
        transform.position = oldPosition + new Vector3(dx, 0, 0);
    }
}
