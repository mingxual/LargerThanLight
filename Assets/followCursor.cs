using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCursor : MonoBehaviour
{
    private Vector3 mousePosition;
    public GameObject keyboardPosition;
    public float moveSpeed = 0.1f;
    

    void Update()
    {      //mousePos
           mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);       
    }
}
