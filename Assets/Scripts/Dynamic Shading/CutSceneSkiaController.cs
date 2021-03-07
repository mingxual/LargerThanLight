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

    private void Awake()
    {
        movementDir = Vector3.zero;
        rightDir = Vector3.Cross(Vector3.up, forwardDir);
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
        else {
            anim.SetBool("IsRunning", false);
        }
    }
    private void SetForwardDir(Vector3 dir) { 
        
    }
}
