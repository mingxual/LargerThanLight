using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopBanging : MonoBehaviour
{
    public AudioSource aS;
    public Animator anim;
        public void StopIt()
    {
        aS.enabled = false;
        anim.enabled = false;
    }
}
