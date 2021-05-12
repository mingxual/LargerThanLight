using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour
{
    public static DontDestroy instance;

    [SerializeField] bool isFirstScene = true;
    [SerializeField] List<GameObject> subPages;
    [SerializeField] GameObject sayDialogue;
    [SerializeField] Slider slide;
    [SerializeField] GameObject mainPageRoot;
    [SerializeField] GameObject controlButton;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        sayDialogue = null;

        if (SceneManager.GetActiveScene().buildIndex == 0)
            SwitchToFirstScene();
        else
            SwitchToNotFirstScene();
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
        mainPageRoot.SetActive(false);
        controlButton.SetActive(true);
    }

    public void SwitchToFirstScene()
    {
        isFirstScene = true;
        DeActiveAllSubPages();
        mainPageRoot.SetActive(true);
        controlButton.SetActive(false);
    }

    public void BackToMainPage()
    {
        SceneManager.LoadScene(0);
        DeActiveAllSubPages();
        isFirstScene = true;
        mainPageRoot.SetActive(true);

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

    public void SetSayDialogue(GameObject inDialogue)
    {
        sayDialogue = inDialogue;
        ChangeVolume();
    }

    public void ChangeVolume()
    {
        if(sayDialogue != null)
        {
            sayDialogue.GetComponent<WriterAudio>().volume = slide.value;
            sayDialogue.GetComponent<AudioSource>().volume = slide.value;
        }
    }

    public void OnClickStartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SwitchToNotFirstScene();
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
