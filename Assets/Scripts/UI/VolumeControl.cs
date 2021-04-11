using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    //public AudioManager audioManager;
    public Slider musicControl;
    public Slider soundControl;

    // Start is called before the first frame update
    void Start()
    {
        //audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioManager.instance)
        {
            AudioManager.instance.MusicVolumeChange(musicControl.value);
            AudioManager.instance.SoundVolumeChange(soundControl.value);
        }
    }
}
