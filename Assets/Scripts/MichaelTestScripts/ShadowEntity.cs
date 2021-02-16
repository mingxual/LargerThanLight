using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEntity : SCObstacle
{
    void CollisionTrigger(GameObject skia)
    {
        skia.GetComponent<SimpleController>().ResetSkia();
    }
}
