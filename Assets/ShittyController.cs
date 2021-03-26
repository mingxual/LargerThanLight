using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShittyController : MonoBehaviour
{
    public Animator mAnim;
    public float speed = 5;
    public GameObject mShit;
    Vector3 mLeftFacingDirection;
    Vector3 mRightFacingDirection;

    private void Start()
    {
        mLeftFacingDirection = transform.localScale;
        mLeftFacingDirection.x *= -1.0f;
        mRightFacingDirection = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            mAnim.SetBool("isRunning", true);
            transform.Translate(Vector3.right * Time.deltaTime * speed); //L
            transform.localScale = mLeftFacingDirection;

        }

        if (Input.GetKey(KeyCode.D))
        {
            mAnim.SetBool("isRunning", true);
            transform.Translate(Vector3.left * Time.deltaTime * speed); //R
            transform.localScale = mRightFacingDirection;
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            mAnim.SetBool("isRunning", false);
        }
    }
}
