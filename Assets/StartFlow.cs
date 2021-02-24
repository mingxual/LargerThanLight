using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class StartFlow : MonoBehaviour
{
    public Flowchart flowy;
    public AudioSource source;
    public AudioClip screech;
    public AudioClip smash;


    public void MsgSend()
    {
        flowy.SendFungusMessage("StartEnd");
    }
    public void PlayScreech()
    {
        source.PlayOneShot(screech);
    }
    public void PlaySmash()
    {
        source.PlayOneShot(smash);
    }

}
