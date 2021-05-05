﻿using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public Music[] musics;

    public static AudioManager instance;

    private float BGTimer = 89f;

    private int level = 0;

    private int loopNum = 0;

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

    private void Update()
    {
        if(level==1)
        {
            LockerUpdate();
        }
        else if (level == 2)
        {
            TheaterUpdate();
        }
        else if (level == 3)
        {
            GymUpdate();
        }
    }

    //
    //Basic Functionalities-----------------------------------------------
    //
    public void PlayOnce(string name, Vector3 place)
    {
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

    public void PlayMusicOnce(string name, int volume)
    {
        Music m = Array.Find(musics, music => music.name == name);
        if (m == null)
        {
            Debug.LogWarning("Music " + name + " not found");
            return;
        }
        if (volume == -1)
        {
            m.source.volume = m.volume;
            m.source.PlayOneShot(m.source.clip);
        }
        else if (volume == 2)
        {
            m.source.PlayOneShot(m.source.clip);
        }
        else if(volume>=0 && volume<=1)
        {
            m.source.PlayOneShot(m.source.clip, volume);
        }
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

    public void MusicVolumeChange(float newVolume)
    {
        foreach (Music m in musics)
        {
            m.volume = newVolume;
            //special case for theater level
            if(m.name=="Theater_Layer_1" || m.name == "Theater_Layer_2"|| m.name == "Theater_Layer_3")
            {
                if(m.source.volume==0)
                {
                    continue;
                }
            }
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


    //
    //Level Related Functions-----------------------------------------------
    //
    public void LevelChange(int inLevel)
    {
        level = inLevel;
        if(level==1)
        {
            InitializeLockerLevel();
        }
        else if(level==2)
        {
            InitializeTheaterLevel();
        }
    }

    public void InitializeMenu()
    {
        foreach (Music m in musics)
        {
            m.source.Stop();
        }
        PlayMusic("Menu");
    }

    public void InitializeLockerLevel()
    {
        if (level != 1)
        {
            level = 1;
            foreach (Music m in musics)
            {
                m.source.Stop();
            }
            PlayMusicOnce("Intro_Loop_1", -1);
            BGTimer = GetMusic("Intro_Loop_1").source.clip.length - 8.67f;
            loopNum = 1;
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
            UpdateLayer(1);
            PlayMusicOnce("Theater_Layer_1", 2);
            PlayMusicOnce("Theater_Layer_2", 2);
            PlayMusicOnce("Theater_Layer_3", 2);

            BGTimer = GetMusic("Theater_Layer_1").source.clip.length - 7.06f;
        }
    }

    public void InitializeGymLevel()
    {
        if (level != 3)
        {
            level = 3;
            foreach (Music m in musics)
            {
                m.source.Stop();
            }
            loopNum = 0;
        }
    }

    public void TheaterUpdate()
    {
        BGTimer -= Time.deltaTime / Time.timeScale;
        if (BGTimer <= 0f)
        {
            PlayMusicOnce("Theater_Layer_1", 2);
            PlayMusicOnce("Theater_Layer_2", 2);
            PlayMusicOnce("Theater_Layer_3", 2);

            BGTimer = GetMusic("Theater_Layer_1").source.clip.length - 7.06f;
        }
    }

    public void UpdateLayer(int layerNumber)
    {
        Music layer1 = GetMusic("Theater_Layer_1");
        Music layer2 = GetMusic("Theater_Layer_2");
        Music layer3 = GetMusic("Theater_Layer_3");
        if (layerNumber == 1)
        {
            layer1.source.volume = layer1.volume;
            layer2.source.volume = 0;
            layer3.source.volume = 0;
        }
        else if (layerNumber == 2)
        {
            layer1.source.volume = 0;
            layer2.source.volume = layer2.volume;
            layer3.source.volume = 0;
        }
        else if (layerNumber == 3)
        {
            layer1.source.volume = 0;
            layer2.source.volume = 0;
            layer3.source.volume = layer3.volume;
        }
    }


    public void LockerUpdate()
    {
        BGTimer -= Time.deltaTime / Time.timeScale;
        if (BGTimer <= 0f)
        {
            if (loopNum == 1)
            {
                PlayMusicOnce("Intro_Loop_1", -1);
                BGTimer = GetMusic("Intro_Loop_1").source.clip.length - 8.67f;
            }
            else if (loopNum == 2)
            {
                PlayMusicOnce("Intro_Loop_2", -1);
                BGTimer = GetMusic("Intro_Loop_2").source.clip.length - 8.67f;
            }
            else if (loopNum == 3)
            {
                PlayMusicOnce("Intro_Loop_3", -1);
                BGTimer = GetMusic("Intro_Loop_3").source.clip.length - 8.67f;
            }
            else if (loopNum == 4)
            {
                PlayMusicOnce("Intro_Loop_4", -1);
                BGTimer = GetMusic("Intro_Loop_4").source.clip.length - 8.67f;
            }
        }
    }

    public IEnumerator LockerMusicUpdate(int inLoop)
    {
        PlayMusicOnce("Intro_Transition", -1);
        loopNum = inLoop;
        Music temp;
        if (inLoop == 2)
        {
            temp = GetMusic("Intro_Loop_1");
            StartCoroutine(Fade(temp.source, 2.89f, 0));
            yield return new WaitForSeconds(2.89f);
            StopMusic("Intro_Loop_1");
            PlayMusicOnce("Intro_Loop_2", -1);
            BGTimer = GetMusic("Intro_Loop_2").source.clip.length - 8.67f;
        }
        else if (inLoop == 3)
        {
            temp = GetMusic("Intro_Loop_2");
            StartCoroutine(Fade(temp.source, 2.89f, 0));
            yield return new WaitForSeconds(2.89f);
            StopMusic("Intro_Loop_2");
            PlayMusicOnce("Intro_Loop_3", -1);
            BGTimer = GetMusic("Intro_Loop_3").source.clip.length - 8.67f;
        }
        else if (inLoop == 4)
        {
            temp = GetMusic("Intro_Loop_3");
            StartCoroutine(Fade(temp.source, 2.89f, 0));
            yield return new WaitForSeconds(2.89f);
            StopMusic("Intro_Loop_3");
            PlayMusicOnce("Intro_Loop_4", -1);
            BGTimer = GetMusic("Intro_Loop_4").source.clip.length - 8.67f;
        }
    }

    public void GymUpdate()
    {
        BGTimer -= Time.deltaTime / Time.timeScale;
        if (BGTimer <= 0f)
        {
            if (loopNum == 1)
            {
                PlayMusicOnce("Final_Intro", -1);
                BGTimer = GetMusic("Final_Intro").source.clip.length - 7.5f;
            }
            else if (loopNum == 2)
            {
                PlayMusicOnce("Final_Boss", -1);
                BGTimer = GetMusic("Final_Boss").source.clip.length - 7.5f;
            }
            else if (loopNum == 3)
            {
                //do nothing
            }
            else if (loopNum == 4)
            {
                PlayMusicOnce("Final_Cutscene", -1);
                BGTimer = GetMusic("Final_Cutscene").source.clip.length - 8.67f;
            }
            else if (loopNum == 5)
            {
                //do nothing
            }
        }
    }

    public IEnumerator GymMusicUpdate(int inLoop)
    {
        Music temp;
        Music temp2;
        loopNum = inLoop;
        if (inLoop == 1)
        {
            PlayMusicOnce("Final_Intro", -1);
            BGTimer = GetMusic("Intro_Loop_1").source.clip.length - 7.5f;
        }
        else if (inLoop == 2)
        {
            temp = GetMusic("Final_Intro");
            temp2 = GetMusic("Final_Boss");
            temp2.source.volume = 0;
            StartCoroutine(Fade(temp.source, 3f, 0));
            PlayMusicOnce("Final_Boss", -1);
            StartCoroutine(Fade(temp2.source, 3f, temp2.volume));
            BGTimer = GetMusic("Final_Boss").source.clip.length - 7.5f;
        }
        else if (inLoop == 3)
        {
            temp = GetMusic("Final_Boss");
            temp2 = GetMusic("Final_Transition");
            temp2.source.volume = 0;
            StartCoroutine(Fade(temp.source, 2f, 0));
            PlayMusicOnce("Final_Transition", -1);
            StartCoroutine(Fade(temp2.source, 2f, temp2.volume));
        }
        else if (inLoop == 4)
        {
            PlayMusicOnce("Final_Cutscene", -1);
            BGTimer = GetMusic("Final_Cutscene").source.clip.length - 8.67f;
        }
        else if (inLoop == 5)
        {
            PlayMusicOnce("Intro_Transition", -1);
            temp = GetMusic("Final_Cutscene");
            StartCoroutine(Fade(temp.source, 2.89f, 0));
            yield return new WaitForSeconds(2.89f);
            StopMusic("Final_Cutscene");
            PlayMusicOnce("End_Credits", -1);
        }
    }


    public IEnumerator Fade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

}
