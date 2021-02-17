using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCam : MonoBehaviour
{
    public Camera gameCam;
    public GameObject startCamera;
    public SkinnedMeshRenderer skiaMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CameraSwitch()
    {
        gameCam.enabled = true;
        skiaMeshRenderer.enabled = true;
        Destroy(startCamera);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
