using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip[] clips;

    [Range(0f,1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop= false;

    [HideInInspector]
    public AudioSource source;

    public void PlayOnce(int whichOne, Vector3 place)
    {
        if (whichOne > clips.Length || whichOne < 0)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }
        else if (whichOne==0)
        {
            int temp = Random.Range(0, clips.Length);
            AudioSource.PlayClipAtPoint(clips[temp], place, source.volume);
            //source.PlayOneShot(clips[temp],volume);
        }
        else {
            AudioSource.PlayClipAtPoint(clips[whichOne - 1], place, source.volume);
            //source.PlayOneShot(clips[whichOne - 1],volume);
        }
    }
}
