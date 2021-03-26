using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToOrigin : MonoBehaviour
{
    public Vector3 target;
    public float speed = 1.0f;
    public SkinnedMeshRenderer thePlayerMesh;
    public SimpleController m_SimpleController;
    public GameObject respawnAnimation;
    // Start is called before the first frame update
    void Start()
    {
        GameObject thePlayerMesh = GameObject.Find("Player2D");
        //SimpleController playerScript = thePlayer.GetComponent<SimpleController>();
        //target = playerScript.originalPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target) < 0.001f)
        {
            // Swap the position of the cylinder.
            target *= -1.0f;
            Instantiate(respawnAnimation, transform.position, transform.rotation);
            thePlayerMesh.enabled = true;
            m_SimpleController.enabled = true;
            Destroy(gameObject);
        }

    }
}
