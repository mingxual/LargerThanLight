using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
public class startSpid : MonoBehaviour
{
    public GameObject spiderSprite;
    public GameObject spiderSculpture;
    public bool spiderflowON = false;
    public Flowchart transFlow;
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
            spiderflowON = true;
            transFlow.SendFungusMessage("SpiderJump");

        }

        if(spiderflowON == true)
        {
            transFlow.SendFungusMessage("SpiderJump");
            spiderflowON = false;
            Destroy(this);
        }
    }

}
