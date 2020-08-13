using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Dialogue : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        // to do
        // if (StoryController.)
        {
            Flowchart.BroadcastFungusMessage("DialogueEvent");
        }
    }
}
