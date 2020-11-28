using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SpawnpointTrigger : MonoBehaviour
{
    Collider2D col2D;

    void Start()
    {
        col2D = GetComponent<Collider2D>();
        col2D.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SimpleController>())
        {
            collision.GetComponent<SimpleController>().SetNewCheckpoint(transform.position);
        }
    }
}
