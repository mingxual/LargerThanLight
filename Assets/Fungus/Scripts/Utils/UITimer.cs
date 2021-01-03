using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    public float time = 3f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Hide", time);
    }

    // Update is called once per frame
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
