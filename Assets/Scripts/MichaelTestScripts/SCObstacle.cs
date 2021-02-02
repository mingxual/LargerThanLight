using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCObstacle : MonoBehaviour
{
    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Obstacle");
    }
}
