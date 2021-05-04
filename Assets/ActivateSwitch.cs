using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSwitch : MonoBehaviour
{
    public Animator myAnim;
    public GameObject wire;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = this.gameObject.GetComponent<Animator>();   
    }

    // Update is called once per frame
    public void TurnMeOn( GameObject nextTrig)
    {
        myAnim.SetBool("TurnOn", true);
        nextTrig.SetActive(true);
    }
    public void WireCreate()
    {
        wire.SetActive(true);
    }

}
