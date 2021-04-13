using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLux : MonoBehaviour
{
    public GameObject lux;
    public float rightBorder;
    public float leftBorder;

    public GameObject rightUI;
    public GameObject leftUI;

    void Update()
    {
        if (GetComponent<Camera>().WorldToScreenPoint(lux.transform.position).x > (Screen.width / 2) + rightBorder)
        {
            Debug.Log("Lux is right of the middle of the screen.");
            rightUI.SetActive(true);
            leftUI.SetActive(false);


        }
        else if (GetComponent<Camera>().WorldToScreenPoint(lux.transform.position).x < (Screen.width / 2) - leftBorder)
        {
            Debug.Log("Lux is left of the middle of the screen.");
            leftUI.SetActive(true);
            rightUI.SetActive(false);

        }
        else
        {
            leftUI.SetActive(false);
            rightUI.SetActive(false);

        }

    }
}

