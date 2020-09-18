using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl_Audio : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    private AudioSource footstep;

    private void Start()
    {
        footstep=gameObject.GetComponent<AudioSource>();
        footstep.loop = true;
        footstep.Play();
        footstep.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * moveSpeed * Time.deltaTime;
        }

        //temporary sound code
        if(Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.RightArrow)||
            Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.DownArrow))
        {
            if(!footstep.isPlaying)
            {
                footstep.UnPause();
            }
        }
        else
        {
            if(footstep.isPlaying)
            {
                footstep.Pause();
            }
        }

        /*if (Input.GetKey(KeyCode.L))
        {
            transform.Rotate(0.0f, rotateSpeed * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(KeyCode.J))
        {
            transform.Rotate(0.0f, -rotateSpeed * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(KeyCode.I))
        {
            transform.Rotate(-rotateSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.K))
        {
            transform.Rotate(rotateSpeed * Time.deltaTime, 0.0f, 0.0f);
        }*/
    }
}
