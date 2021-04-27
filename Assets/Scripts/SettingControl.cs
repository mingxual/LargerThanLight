using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingControl : MonoBehaviour
{
    public List<UniversalRenderPipelineAsset> renderPipelineAssets;
    [SerializeField] Dropdown dn;

    // Start is called before the first frame update
    void Start()
    {
        dn.value = renderPipelineAssets.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchLevel()
    {
        QualitySettings.SetQualityLevel(dn.value);
        GraphicsSettings.renderPipelineAsset = renderPipelineAssets[dn.value];
    }
}
