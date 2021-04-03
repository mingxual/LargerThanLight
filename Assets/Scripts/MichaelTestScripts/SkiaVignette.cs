using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Fungus;
public class SkiaVignette : MonoBehaviour
{
    public float lightStatus;
    public float squishStatus;
    [SerializeField] Volume volume;
    Vignette vignette;
    ChromaticAberration chAbr;
    public SimpleController skiaCtrlr;
    public Flowchart mFlowchart;

    public float invSpeed;

    public float chrabrValue;

    void Start()
    {
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out chAbr);
        skiaCtrlr = GetComponent<SimpleController>();
    }

    void Update()
    {
        chrabrValue = chAbr.intensity.value;
        CheckLightStatus();
        if(skiaCtrlr.IsDead() == true)
        {
            SkiaDiedStuff();
        }
    }

    void SkiaDiedStuff()
    {
        mFlowchart.SendFungusMessage("Owch");
        if (chAbr.intensity.value < 1)
        {
            chAbr.intensity.value += 0.25f;
        }
        else
        {
            chAbr.intensity.value = 0;
            skiaCtrlr.SetDeadStatus(false);

        }

    }

    void CheckLightStatus()
    {
        if (!vignette) return;

        float status = Mathf.Max(lightStatus, squishStatus);
        if (vignette.intensity.value == 0 && status == 0) return;

        print("vignette active");

        float sin = 0.5f + 0.5f * Mathf.Sin(Time.time * invSpeed);
        vignette.color.value = new Color(0.039f * sin, 0, 0.059f * sin);
        vignette.smoothness.value = 0.75f + 0.15f * sin;
        if (vignette.intensity.value < status)
        {
            vignette.intensity.value += Time.deltaTime / Time.timeScale;
            if (vignette.intensity.value > status)
                vignette.intensity.value = status;
        }
        else if (vignette.intensity.value > status)
        {
            vignette.intensity.value -= Time.deltaTime * 2 / Time.timeScale;
            if (vignette.intensity.value < status)
                vignette.intensity.value = status;
        }
       
    }
    
   
}
