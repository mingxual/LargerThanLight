using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startSpid : MonoBehaviour
{
    public GameObject spiderSprite;
    public GameObject spiderSculpture;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.transform.position.x <= -212f)
        {
            spiderSprite.SetActive(true);
            spiderSculpture.SetActive(false);

        }
    }

}
