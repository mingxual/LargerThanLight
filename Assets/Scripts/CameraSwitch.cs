using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public List<GameObject> cameraPoses;
    public GameObject originCamera;
    public float transitionTime;
    public List<GameObject> cameraWalls;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        LeanTween.move(originCamera, cameraPoses[0].transform.position, transitionTime).setOnComplete(FadeCameraWalls);
    }

    public void ChangeToCamera(int index)
    {
        if(index >= 0)
        {
            LeanTween.move(originCamera, cameraPoses[index].transform.position, transitionTime);
        }
    }

    public void FadeCameraWalls()
    {
        foreach (GameObject go in cameraWalls)
        {
            // I cannot get the walls to fade out properly
            // the banners go invisible in front of the material
            go.SetActive(false);
        }
    }
}
