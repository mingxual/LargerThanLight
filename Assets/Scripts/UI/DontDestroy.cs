using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    public GameObject controlRoot;
    public bool isFirstScene = true;

    [SerializeField] List<GameObject> subChildren;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isFirstScene)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (controlRoot.activeInHierarchy == false)
                {
                    controlRoot.SetActive(true);
                    // Time.timeScale = 0;
                }
                else
                {
                    controlRoot.SetActive(false);
                    // Time.timeScale = 1;
                }
            }
        }
    }

    public void SwitchToNotFirstScene()
    {
        isFirstScene = false;

        for (int i = 0; i < subChildren.Count; i++)
            subChildren[i].SetActive(true);

        controlRoot.SetActive(false);
    }

    /*
    public void QuiteApp()
    {
        Application.Quit();
    }
    public void CloseOptions()
    {
        controlRoot.SetActive(false);
        // Time.timeScale = 1;
    }
    */
}
