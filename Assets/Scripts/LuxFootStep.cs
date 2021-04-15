using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuxFootStep : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayLuxFootstep()
    {
        AudioManager.instance.PlayOnce("Lux_Footstep", gameObject.transform.position);
    }
}
