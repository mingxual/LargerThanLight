using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ShadowDissapear : MonoBehaviour
{
    public SkinnedMeshRenderer mSkiaRenderer;


    public void ShadowGone()
    {
        mSkiaRenderer.shadowCastingMode = ShadowCastingMode.Off;
    }
}
