using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SkiaVignette : MonoBehaviour
{
    [HideInInspector] public float lightStatus;
    [HideInInspector] public float squishStatus;
    [SerializeField] Volume volume;
    Vignette vignette;

    public float invSpeed;

    void Start()
    {
        volume.profile.TryGet(out vignette);
    }

    void Update()
    {
        CheckLightStatus();
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
            vignette.intensity.value += Time.deltaTime;
            if (vignette.intensity.value > status)
                vignette.intensity.value = status;
        }
        else if (vignette.intensity.value > status)
        {
            vignette.intensity.value -= Time.deltaTime * 2;
            if (vignette.intensity.value < status)
                vignette.intensity.value = status;
        }
    }
}
