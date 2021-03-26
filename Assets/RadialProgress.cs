using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RadialProgress : MonoBehaviour
{
    public Image LoadingBar;
    public float currentValue = 0f;
    public float speedUP = 2;
    public float speedDOWN = 0.5f;


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            currentValue += speedUP;
        }

        if (currentValue <= 100 && currentValue > 0)
        {
            currentValue -= speedDOWN;


        }
        else if (currentValue >= 100f)
        {
            SceneManager.LoadScene("Lockers");
        }
        
        LoadingBar.fillAmount = currentValue / 100;
    }
}
