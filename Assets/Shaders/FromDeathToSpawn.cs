using UnityEngine;
using System.Collections;


public class FromDeathToSpawn : MonoBehaviour
{
    // Adjust the speed for the application.
    //public float speed = 1.0f;
    public float duration = 1.3f;
    private float speed;

    // The respawn position.
    public Transform target;
    //the death position
    public Transform deadPos;

    void Awake()
    {
        // Position the effect at the death point.
        transform.position = deadPos.position;
        speed = Vector3.Distance(target.position, deadPos.position) / duration;
    }

    void Update()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            Destroy(transform.gameObject);
        }
    }
}
