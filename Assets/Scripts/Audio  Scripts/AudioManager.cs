using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public Music[] musics;

    public static AudioManager instance;

    private float BGTimer = 89f;

    private bool isBG=true;

    private int layerNumber = 1;

    private int level = 0;

    private Music layer1;
    private Music layer2;
    private Music layer3;

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
            s.source.clip = s.clips[0];//just for name 

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

    public void Start()
    {
        InitializeMenu();
    }

    public void Update()
    {
        if (level == 2)
        {
            TheaterUpdate();
        }
    }

    public void PlayOnce(string name, Vector3 place)
    {
        /*
        if (name == "Lux_Footstep")
        {
            int index = UnityEngine.Random.Range(0, 4);
            string realName = name + "_" + index;
            s = Array.Find(sounds, sound => sound.name == realName);
            s.source.PlayOneShot(s.source.clip);
        }
        else if (name == "Lux_Flicker")
        {
            int index = UnityEngine.Random.Range(0, 6);
            string realName = name + "_" + index;
            s = Array.Find(sounds, sound => sound.name == realName);
            s.source.PlayOneShot(s.source.clip);
        }*/
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + " not found");
            return;
        }
        s.PlayOnce(0,place);
    }

    public void PlayOnceNoPlace(string name)
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

    public void UpdateLayer(int inLayer)
    {
        layerNumber = inLayer;
        if(layerNumber == 1)
        {
            layer1.source.volume = layer1.volume;
            layer2.source.volume = 0f;
            layer3.source.volume = 0f;
        }
        else if (layerNumber == 2)
        {
            layer2.source.volume = layer2.volume;
            layer1.source.volume = 0f;
            layer3.source.volume = 0f;
        }
        else if (layerNumber == 3)
        {
            layer3.source.volume = layer3.volume;
            layer1.source.volume = 0f;
            layer2.source.volume = 0f;
        }
    }

    public void MusicVolumeChange(float newVolume)
    {
        foreach (Music m in musics)
        {
            m.source.volume = newVolume;
        }
    }

    public void SoundVolumeChange(float volumeFactor)
    {
        foreach (Sound s in sounds)
        {
            float temp = s.volume * (volumeFactor * 2);
            if(temp > 1)
            {
                temp = 1;
            }
            s.source.volume = temp;
        }
    }

    public void LevelChange(int inLevel)
    {
        level = inLevel;
        if(level==2)
        {
            InitializeTheaterLevel();
        }
    }

    public void InitializeTheaterLevel()
    {
        if (level != 2)
        {
            level = 2;
            foreach (Music m in musics)
            {
                m.source.Stop();
            }
            layer1 = GetMusic("Theater_Layer_1");
            layer2 = GetMusic("Theater_Layer_2");
            layer3 = GetMusic("Theater_Layer_3");
            layer2.source.volume = 0;
            layer3.source.volume = 0;
            layer1.source.Play();
            layer2.source.Play();
            layer3.source.Play();
            BGTimer = layer1.source.clip.length - 7.06f;
        }
    }

    public void InitializeGymLevel()
    {
        if (level != 3)
        {
            level = 3;
            layer1 = GetMusic("Theater_Layer_1");
            layer2 = GetMusic("Theater_Layer_2");
            layer3 = GetMusic("Theater_Layer_3");
            layer1.source.volume = 0;
            layer2.source.volume = 0;
            layer3.source.volume = layer3.volume;
        }
    }

    public void InitializeMenu()
    {
        PlayMusic("The_Adventure");
    }

    public void TheaterUpdate()
    {
        BGTimer -= Time.deltaTime;
        if (BGTimer <= 0f)
        {
            layer1.source.Play();
            layer2.source.Play();
            layer3.source.Play();
            BGTimer = layer1.source.clip.length - 7.06f;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isBG)
            {
                isBG = false;
                if (layerNumber == 1)
                {
                    layer1.source.volume = 0f;
                }
                else if (layerNumber == 2)
                {
                    layer2.source.volume = 0f;
                }
                else if (layerNumber == 3)
                {
                    layer3.source.volume = 0f;
                }
            }
            else
            {
                isBG = true;
                if (layerNumber == 1)
                {
                    layer1.source.volume = layer1.volume;
                }
                else if (layerNumber == 2)
                {
                    layer2.source.volume = layer2.volume;
                }
                else if (layerNumber == 3)
                {
                    layer3.source.volume = layer3.volume;
                }
            }

        }
    }
}
