using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightOutEffect : MonoBehaviour
{
    Image LightImage;
    private float alpha;
    private bool activate;

    // Start is called before the first frame update
    void Start()
    {
        alpha = 0;
        activate = false;
        LightImage = GetComponent<Image>();
        LightImage.color = new Color(1, 1, 1, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        if (activate)
        {
            alpha += Time.deltaTime;
            LightImage.color = new Color(1, 1, 1, alpha);
        }
    }

    public void SetActivate(bool active)
    {
        activate = active;
    }
}
