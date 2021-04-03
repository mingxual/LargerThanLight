using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkMaskController : MonoBehaviour
{
    public Slider slide;
    public Image frame;
    private float ratio;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        slide.value = slide.maxValue;
        ratio = 1.0f;
        AdjustToRightAmount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBrightness()
    {
        ratio = slide.value;
        AdjustToRightAmount();
    }

    private void AdjustToRightAmount()
    {
        Color currColor = frame.color;
        currColor.a = 1.0f - ratio;
        frame.color = currColor;
    }
}
