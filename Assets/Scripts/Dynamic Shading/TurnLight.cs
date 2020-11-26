using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnLight : MonoBehaviour
{
    bool turnOnLight;

    // Start is called before the first frame update
    void Start()
    {
        turnOnLight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            turnOnLight = !turnOnLight;
            GetComponent<Light>().enabled = turnOnLight;
        }
    }
}
