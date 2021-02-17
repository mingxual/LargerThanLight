using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GymManager : MonoBehaviour
{
    private bool active;
    private int towerflag;

    [SerializeField] GameObject floodlights;

    private void Start()
    {
        active = true;
        ResetTowerFlag();
    }

    public void TowerActivate(int index)
    {
        if (!active) return;
        Debug.Log("activating tower: " + index);
        switch (index)
        {
            case 1: towerflag |= 1; break;
            case 2: towerflag |= 2; break;
            case 3: towerflag |= 4; break;
            default: return;
        }

        CheckTowerFlag();
    }

    private void ResetTowerFlag()
    {
        towerflag = 0;
    }

    private void CheckTowerFlag()
    {
        if (towerflag >= 7)
        {
            //Debug.Log("DONE");
            Invoke("ActivateFloodLights", 2f);
            active = false;
        }
    }
    
    private void ActivateFloodLights()
    {
        floodlights.SetActive(true);
        Invoke("EndScene", 2f);
    }

    private void EndScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
