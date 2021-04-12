using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInitializer : MonoBehaviour
{
    public void InitializeLocker()
    {
        AudioManager.instance.InitializeLockerLevel();
    }
    public void InitializeTheater()
    {
        AudioManager.instance.InitializeTheaterLevel();
    }
    public void InitializeGym()
    {
        AudioManager.instance.InitializeGymLevel();
    }

    public void UpdateLayer(int layer)
    {
        AudioManager.instance.UpdateLayer(layer);
    }

    public void LockerMusicUpdate(int inLoop)
    {
        StartCoroutine(AudioManager.instance.LockerMusicUpdate(inLoop));
    }

    public void PlayOnceNoPlace(string name)
    {
        AudioManager.instance.PlayOnceNoPlace(name);
    }
}
