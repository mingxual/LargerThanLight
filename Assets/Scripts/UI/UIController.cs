using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Camera cam;
    public SimpleController skia;
    public LightController lux;

    // Image frame that would hold the images
    public Image frame;

    // An array of UI pictures to show
    public List<Sprite> images;
    // An array of mode
    public List<int> modes;

    // Offset to the screen position
    public Vector3 skia_offset;
    public Vector3 lux_offset;

    // Mode control
    public float maximumDiaplayTime = 5f;
    private float timer;
    public int mode;
    public bool isDisplay;
    public bool isAttachedToSkia;

    // Start is called before the first frame update
    void Start()
    {
        mode = -1;
        isDisplay = false;
        isAttachedToSkia = true;
        frame.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isDisplay)
        {
            timer += Time.deltaTime;

            if(timer >= maximumDiaplayTime)
            {
                StartCoroutine(FadeUI());
            }

            switch(mode)
            {
                case 1:
                    FirstMode();
                    break;

                case 2:
                    SecondMode();
                    break;

                case 3:
                    ThirdMode();
                    break;

                default:
                    break;
            }
        }
    }

    // Return the skia's screen position relative to the cam
    public Vector3 GetScreenPos()
    {
        if (isAttachedToSkia)
        {
            return cam.WorldToScreenPoint(skia.GetWorldPosition3D()) + skia_offset;
        }
        else
        {
            return cam.WorldToScreenPoint(lux.transform.position) + lux_offset;
        }
    }

    public void DisplayUI(int index)
    {
        if (index < 0 || index >= images.Count)
            Debug.LogError("The index passed is wrong");

        frame.sprite = images[index];
        mode = modes[index];
        frame.gameObject.SetActive(true);
        isDisplay = true;

        frame.rectTransform.sizeDelta = new Vector2((images[index].rect.width/images[index].rect.height) * frame.rectTransform.rect.height, frame.rectTransform.rect.height);
        
        Vector3 framePos = GetScreenPos();
        framePos.z = 0;
        frame.transform.position = framePos;

        // reset timer
        timer = 0.0f;
    }

    public void CloseUI()
    {
        frame.gameObject.SetActive(false);
    }

    public void SwitchCamera(Camera next)
    {
        cam = next;
    }

    // Skia movement
    private void FirstMode()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(FadeUI());
        }
    }

    // Skia interaction
    private void SecondMode()
    {
        if (Input.GetAxis("Interaction") > 0.8f)
        {
            StartCoroutine(FadeUI());
        }
    }

    // Lux movement
    private void ThirdMode()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(FadeUI());
        }
    }

    public void AttachToSkia(bool val)
    {
        isAttachedToSkia = val;
    }

    IEnumerator FadeUI()
    {
        float elasped_time = 0.0f;
        isDisplay = false;

        Color frame_color;

        while (elasped_time < 1.0f)
        {
            elasped_time += Time.deltaTime;
            frame_color = frame.color;
            frame_color.a = 1.0f - elasped_time;
            frame.color = frame_color;
            yield return null;
        }

        CloseUI();
        frame_color = frame.color;
        frame_color.a = 1.0f;
        frame.color = frame_color;

        yield return null;
    }
}
