using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerr : MonoBehaviour
{

    public GameObject OptionsUI;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionsUI.activeInHierarchy == false)
            { 
            OptionsUI.SetActive(true);
            Time.timeScale = 0;
            }
            else
            {
                OptionsUI.SetActive(false);
                Time.timeScale = 1;
            }
        }

    }
    public void QuiteApp()
    {
        Application.Quit();
    }
    public void CloseOptions()
    {
        OptionsUI.SetActive(false);
        Time.timeScale = 1;


    }
}
