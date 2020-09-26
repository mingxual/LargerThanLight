using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public Music[] musics;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (Music m in musics)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }
    }

    public void PlayOnce(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }
        s.source.PlayOneShot(s.source.clip);
    }

    public void PlayMusic(string name)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            Debug.LogWarning("Music " + name + " not found");
            return;
        }
        m.source.Play();
    }

    public void StopMusic(string name)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            Debug.LogWarning("Music " + name + " not found");
            return;
        }
        m.source.Stop();
    }
    
    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return null;
        }
        return s;
    }

    public Music GetMusic(string name)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            Debug.LogWarning("Music " + name + " not found");
            return null;
        }
        return m;
    }
}
