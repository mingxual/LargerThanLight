using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public float time = 5f;
    public bool keyPressed = false;
    public Animator uiAnimator;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Hide", time);
    }

    private void Update()
    {
        if (keyPressed)
        {
            if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) ||Input.GetKey(KeyCode.DownArrow))
            {
                //gameObject.SetActive(false);
                uiAnimator.SetBool("startFade", true);
            }
        }
    }

    // Update is called once per frame
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
