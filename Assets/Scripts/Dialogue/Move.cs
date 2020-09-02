using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class Move : MonoBehaviour
{
    public float speed; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var sayDialog = SayDialog.GetSayDialog();
        if (sayDialog.isActiveAndEnabled)
        {
            return;
        }
      

        if(Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }
}
