using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudioManager : MonoBehaviour
{
    public AudioSource randomSound;

    public AudioClip[] audioSources;

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
        Debug.Log("PrintEvent: " + " called at: " + Time.time);
        randomSound.clip = audioSources[Random.Range(0, audioSources.Length)];
        randomSound.Play();
    }
}
