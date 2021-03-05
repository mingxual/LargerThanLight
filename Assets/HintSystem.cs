using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class HintSystem : MonoBehaviour
{
    public float time;
    public Flowchart flowchart;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HintTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            Destroy(this);
        }
        
    }
    IEnumerator HintTimer()
    {
        yield return new WaitForSeconds(time);
        flowchart.SendFungusMessage("ShowHint");
    }
}
