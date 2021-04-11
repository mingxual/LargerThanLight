using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public string levelName;

    public void LoadLevel(string levelName)
    {
        StartCoroutine(AsynchronousLoad(levelName));
    }
    IEnumerator AsynchronousLoad(string scene)
    {
        yield return null;
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;
        loadingScreen.SetActive(true);

        while (!ao.isDone)
        {
            // [0, 0.9] > [0, 1]
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            // Loading completed          
            ao.allowSceneActivation = true;
            yield return null;
        }
    }
}
