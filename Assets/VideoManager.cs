using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public List<GameObject> slideList = new List<GameObject>();

    public GameObject videoClip;
    public int ccurrentClup;

    private void Start()
    {
        videoClip = slideList[0];
        ccurrentClup = 0;
    }

    public void NextSlide()
    {
        Debug.Log("Next");
        slideList[ccurrentClup + 2].GetComponent<VideoPlayer>().enabled = true;
        slideList[ccurrentClup + 1].GetComponent<MeshRenderer>().enabled = true;
        slideList[ccurrentClup].GetComponent<MeshRenderer>().enabled = false;
        ccurrentClup++;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            
            NextSlide();
        }
    }
}
