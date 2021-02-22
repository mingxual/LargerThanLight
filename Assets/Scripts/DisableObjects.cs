using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObjects : MonoBehaviour
{
    public List<GameObject> objects;
    public List<SCObstacle> obstacles;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DeactivateObjects()
    {
        // Have to disable obstacle components before objects

        for (int i = 0; i < obstacles.Count; ++i)
        {
            obstacles[i].gameObject.layer = LayerMask.NameToLayer("Default");
            obstacles[i].enabled = false;
        }

        for (int i = 0; i < objects.Count; ++i)
        {
            objects[i].SetActive(false);
        }
    }
}
