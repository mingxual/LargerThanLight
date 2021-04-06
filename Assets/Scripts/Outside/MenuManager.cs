using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public GameObject mainPage;
    public GameObject optionPage;
    public GameObject creditPage;
    public GameObject quitPrompt;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainPage.SetActive(true);
        /*
        optionPage.SetActive(false);
        creditPage.SetActive(false);
        quitPrompt.SetActive(false);
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region MainPage
    public void OnClickStartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnClickOptionBtn()
    {
        optionPage.SetActive(true);
    }

    public void OnClickCreditBtn()
    {
        creditPage.SetActive(true);
    }

    public void OnClickQuitBtn()
    {
        quitPrompt.SetActive(true);
    }

    #endregion

    #region OptionPage
    public void OnClickBackBtn()
    {
        optionPage.SetActive(false);
        creditPage.SetActive(false);
    }

    #endregion

    #region CreditPage
    
    #endregion

    #region QuitPrompt

    public void OnClickConfirmBtn()
    {
        Application.Quit();
    }

    public void OnClickCancelBtn()
    {
        quitPrompt.SetActive(false);
    }
    #endregion
}
