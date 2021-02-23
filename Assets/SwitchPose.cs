using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPose : MonoBehaviour
{
    public GameObject newPose;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DifferentPose()
    {
        newPose.SetActive(true);
        Destroy(this.gameObject);
    }
}
