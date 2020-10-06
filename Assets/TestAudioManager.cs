using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudioManager : MonoBehaviour
{

    private float flicker_timer = 3f;

    private AudioSource Hum;

    // Start is called before the first frame update
    void Start()
    {
        //special case for hum
        Hum = gameObject.AddComponent<AudioSource>();
        Music temp = AudioManager.instance.GetMusic("Lux_Hum");
        Hum.clip = temp.clip;
        Hum.volume = temp.volume;
        Hum.pitch = temp.pitch;
        Hum.loop = temp.loop;
        Hum.spatialBlend = 0.7f;
        Hum.Play();
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
        AudioManager.instance.PlayOnce("Lux_Footstep", gameObject.transform.position);
    }

}
