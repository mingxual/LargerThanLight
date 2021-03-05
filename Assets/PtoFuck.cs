using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class PtoFuck : MonoBehaviour
{
    public Flowchart flowy;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            flowy.SendFungusMessage("FUCK");
        }
    }
}
