using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public float time = 5f;
    public bool key_pressed = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Hide", time);
    }

    void Update()
    {
        if (key_pressed)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                gameObject.SetActive(false);
            }
        }
        
    }

    // Update is called once per frame
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
