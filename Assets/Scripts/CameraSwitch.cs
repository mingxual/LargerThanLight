using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public List<GameObject> cameraPoses;
    private int curIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeToCamera(int index)
    {
        if(curIndex >= 0)
        {
            cameraPoses[curIndex].SetActive(false);
        }

        curIndex = index;

        if(curIndex >= 0)
        {
            cameraPoses[curIndex].SetActive(true);
        }
    }
}
