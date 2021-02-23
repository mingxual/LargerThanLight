using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class SendMsg : MonoBehaviour
{
    public Flowchart mflowchart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WireOne()
    {
        mflowchart.SendFungusMessage("one");
    }
    public void WireTwo()
    {
        mflowchart.SendFungusMessage("two");
    }
    public void WireThree()
    {
        mflowchart.SendFungusMessage("three");
    }
}
