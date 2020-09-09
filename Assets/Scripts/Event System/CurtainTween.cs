using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainTween : MonoBehaviour
{
    public bool isMove = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isMove)
        {
            isMove = true;
            LeanTween.moveY(gameObject, 2.5f, 1.0f);
        }
    }


}
