using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAnimationScript : MonoBehaviour
{
    private Animator skiaAnim;
    // Start is called before the first frame update
    void Start()
    {
        skiaAnim = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.A)))
        {
            skiaAnim.SetBool("IsRunning", true);
        }

        if ((Input.GetKeyUp(KeyCode.D)) || (Input.GetKeyUp(KeyCode.A)))
        {
            skiaAnim.SetBool("IsRunning", false);
        }
    }
}
