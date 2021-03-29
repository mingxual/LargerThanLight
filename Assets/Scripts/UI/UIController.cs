using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Camera cam;
    public SimpleController skia;

    // Image frame that would hold the images
    public Image frame;
    // An array of UI pictures to show
    public List<Sprite> images;
    // Offset to the skia screen position
    public Vector3 offset;
    public float time = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Test
        /*
        Vector3 framePos = GetScreenPos() + offset;
        framePos.z = 0;
        frame.transform.position = framePos;
        */
    }

    // Return the skia's screen position relative to the cam
    public Vector3 GetScreenPos()
    {
        return cam.WorldToScreenPoint(skia.GetWorldPosition3D());
    }

    public void DisplayUI(int index)
    {
        if (index < 0 || index >= images.Count)
            Debug.LogError("The index passed is wrong");

        frame.sprite = images[index];
        frame.gameObject.SetActive(true);
        
        Vector3 framePos = GetScreenPos() + offset;
        framePos.z = 0;
        frame.transform.position = framePos;
        Invoke("CloseUI", time);
    }

    public void CloseUI()
    {
        frame.gameObject.SetActive(false);
    }

    public void SwitchCamera(Camera next)
    {
        cam = next;
    }
}
