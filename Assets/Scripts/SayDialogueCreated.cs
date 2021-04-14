using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SayDialogueCreated : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(DontDestroy.instance != null)
            DontDestroy.instance.SetSayDialogue(gameObject);       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
