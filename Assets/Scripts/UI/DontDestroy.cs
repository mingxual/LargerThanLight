using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    [SerializeField] bool isFirstScene = true;
    [SerializeField] List<GameObject> subPages;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        DeActiveAllSubPages();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFirstScene)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (subPages[0].activeInHierarchy)
                    subPages[0].SetActive(false);
                else
                    subPages[0].SetActive(true);
            }
        }
    }

    private void DeActiveAllSubPages()
    {
        for(int i = 0; i < subPages.Count; i++)
        {
            subPages[i].SetActive(false);
        }
    }

    public void SwitchToNotFirstScene()
    {
        isFirstScene = false;
        DeActiveAllSubPages();
    }

    public void BackToMainPage()
    {
        SceneManager.LoadScene(0);
        DeActiveAllSubPages();

    }
    public void CloseOptions()
    {
        DeActiveAllSubPages();
    }

    public void SwitchToPage(int index)
    {
        DeActiveAllSubPages();
        subPages[index].SetActive(true);
    }
}
