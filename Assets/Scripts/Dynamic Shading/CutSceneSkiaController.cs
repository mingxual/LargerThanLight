using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneSkiaController : MonoBehaviour
{

    [SerializeField] private Animator anim;
    [SerializeField] private Transform skiaModel;
    [SerializeField] Vector3 forwardDir;
    public bool isRunning;
    private Vector3 movementDir;
    private Vector3 rightDir;
    private Vector3 upDir;

    private void Awake()
    {
        movementDir = Vector3.zero;
        rightDir = Vector3.Cross(Vector3.up, forwardDir);
        upDir = Vector3.Cross(rightDir, Vector3.up);
    }
    // Update is called once per frame
    private void Update()
    {
        isRunning = false;
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            isRunning = true;
            skiaModel.LookAt(skiaModel.position - rightDir);
            anim.SetBool("IsRunning", isRunning);
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            isRunning = true;
            skiaModel.LookAt(skiaModel.position + rightDir);
            anim.SetBool("IsRunning", isRunning);
        }

        else if (Input.GetKey(KeyCode.UpArrow))
        {
            isRunning = true;
            skiaModel.LookAt(skiaModel.position + upDir);
            anim.SetBool("IsRunning", isRunning);
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            isRunning = true;
            skiaModel.LookAt(skiaModel.position - upDir);
            anim.SetBool("IsRunning", isRunning);
        }
        else {
            anim.SetBool("IsRunning", false);
        }
    }
}
