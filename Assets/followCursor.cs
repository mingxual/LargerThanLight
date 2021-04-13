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
           /* mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);*/

        //lightPos
        //keyboardPosition = this.transform.position;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            keyboardPosition.transform.Translate(Vector3.up * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            keyboardPosition.transform.Translate(Vector3.down * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            keyboardPosition.transform.Translate(Vector3.left * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            keyboardPosition.transform.Translate(Vector3.right * Time.deltaTime);
        }

        transform.position = Vector2.Lerp(transform.position, keyboardPosition.transform.position, moveSpeed);
    }
}
