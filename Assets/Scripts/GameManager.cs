using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool hasResettedColliderPool = false;

    LevelManager m_LevelManager;

    public LightController luxControl;
    public SimpleController skiaControl;

    public GameObject lux;
    public GameObject skia;

    public GameObject OptionsUI;

    public DataOutput MatricesManager;

    private void Awake()
    {
        // Find LevelManager from scene
        m_LevelManager = FindObjectOfType<LevelManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_LevelManager.InitializeSubLevels();
    }

    
    private void Update()
    { 
        //Debug
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl))
            {
                skia.SendMessage("SkiaDeath");
            }      
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        hasResettedColliderPool = false;
    }

    public void DisableCharacterControl()
    {
        luxControl.enabled = false;
        skiaControl.Disable();
    }

    public void EnableCharacterControl()
    {
        luxControl.enabled = true;
        skiaControl.enabled = true;
    }

    public void SpawnSkiaNextSegment()
    {
        //Debug.Break();
        m_LevelManager.SetNextSegment();

        skia.SendMessage("ResetSkia");
    }

    public void MoveLuxToPos(Transform pos)
    {
        lux.transform.position = pos.position;
    }

    public void MoveSkiaToPos(Transform pos)
    {
        skia.transform.position = pos.position;
    }

    public void SwitchToLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
