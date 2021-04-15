using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundScript : MonoBehaviour
{

    public AudioSource randomSound;

    public AudioClip[] audioSources;

    public AudioClip thisAudio;

    public AudioSource otherSource;

    // Use this for initialization
    void Start()
    {

        CallAudio();
    }


    void CallAudio()
    {
        Invoke("RandomSoundness", Random.Range(1.0f, 10.0f));
    }

    void RandomSoundness()
    {
        randomSound.clip = audioSources[Random.Range(0, audioSources.Length)];
        randomSound.Play();
        CallAudio();
    }

    public void PlayThis()
    {
        otherSource.clip = thisAudio;
        otherSource.Play();
    }
}
