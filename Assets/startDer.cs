using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startDer : MonoBehaviour
{
    public GameObject lux;
    public float pos = -74;
    public GameObject derAnim;
    public GameObject sculptures;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(lux.transform.position.x <= pos)
        {
            derAnim.SetActive(true);
            sculptures.SetActive(false);
        }
    }
}
