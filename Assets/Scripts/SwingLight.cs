using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwingDirection
{
    UpDown,
    LeftRight,
    FrontBack
}

public class SwingLight : MonoBehaviour
{
    [Range(0.05f, 0.3f)]
    public float speed;
    [Range(0.5f, 3.0f)]
    public float distance;
    public SwingDirection direction;

    float radian = 0;
    //float perRadian = 0.15f;
    //float radius = 1.2f;
    Vector3 oldPos;

    // Start is called before the first frame update
    void Start()
    {
        oldPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //floating
        radian += speed;
        float dx = Mathf.Cos(radian) * distance;
        switch (direction)
        {
            case SwingDirection.UpDown:
                transform.position = oldPos + new Vector3(0, dx, 0);
                break;
            case SwingDirection.LeftRight:
                transform.position = oldPos + new Vector3(dx, 0, 0);
                break;
            case SwingDirection.FrontBack:
                transform.position = oldPos + new Vector3(0, 0, dx);
                break;
        }
    }
}
