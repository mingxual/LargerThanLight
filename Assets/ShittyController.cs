using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittyController : MonoBehaviour
{
    public Animator mAnim;
    public float speed = 5;
    public GameObject mShit;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            mAnim.SetBool("isRunning", true);
            transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self); //L
            mShit.transform.Rotate(0, 70f, 0.0f);

        }

        if (Input.GetKey(KeyCode.D))
        {
            mAnim.SetBool("isRunning", true);
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.Self); //R
            mShit.transform.Rotate(0, -100f, 0.0f);


        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            mAnim.SetBool("isRunning", false);
        }
    }
}
