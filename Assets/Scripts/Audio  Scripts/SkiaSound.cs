using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkiaSound : MonoBehaviour
{
    public void PlaySkiaFootstep()
    {
        AudioManager.instance.PlayOnce("Skia_Shadow_Footstep", new Vector3(0,0,0));
        
    }
}
