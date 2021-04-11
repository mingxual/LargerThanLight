using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImHighRightNow : MonoBehaviour
{
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("grow", true);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            anim.SetBool("grow", false);
        }
    }

}
