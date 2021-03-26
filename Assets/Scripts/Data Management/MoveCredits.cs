using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MoveCredits : MonoBehaviour
{
    public void EndScene()
    {
        GameObject existAudioManager = GameObject.Find("AudioManager");
        if(existAudioManager != null)
        {
            Destroy(existAudioManager);
        }
            SceneManager.LoadScene("1STARTHERE");
    }
}
