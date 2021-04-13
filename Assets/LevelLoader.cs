using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public string levelName;

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelName);
    }
    
}
