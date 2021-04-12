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
}
