using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class scare : MonoBehaviour
{
    public Flowchart fc;
    public void DoTheScare()
    {
        fc.SendFungusMessage("SCARE");
    }


}
