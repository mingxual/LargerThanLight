using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowawayScriptYAY : MonoBehaviour
{
    public GameObject luxModel;
    public void Throwaway()
    {
        luxModel.transform.localScale *= 1.8f;
    }
}
