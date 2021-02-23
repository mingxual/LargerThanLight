using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideshowCutsceneManager : MonoBehaviour
{
    public GameObject slideshowImageObject;
    public List<Sprite> slideList = new List<Sprite>();

    private Image slideshowImage;
    private int currentSlide;

    private void Start()
    {
        slideshowImage = slideshowImageObject.GetComponent<Image>();

        slideshowImage.sprite = slideList[0];
        currentSlide = 0;
    }

    public void NextSlide()
    {
        slideshowImage.sprite = slideList[currentSlide + 1];
        currentSlide++;
    }
}
