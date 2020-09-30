using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudioManager : MonoBehaviour
{
    public AudioSource randomSound;

    public AudioClip[] audioSources;

    private float flicker_timer = 3f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(flicker_timer < 0)
        {
            flicker_timer = Random.Range(5f, 10f);
            AudioManager.instance.PlayOnce("Lux_Flicker",gameObject.transform.position);
        }
        flicker_timer -= Time.deltaTime;
    }
    public void PlayLuxFootstep()
    {
        /*
        Debug.Log("PrintEvent: " + " called at: " + Time.time);
        randomSound.clip = audioSources[Random.Range(0, audioSources.Length)];
        randomSound.Play();
        */
        AudioManager.instance.PlayOnce("Lux_Footstep", gameObject.transform.position);
    }

}
