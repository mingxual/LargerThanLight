using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MoveCredits : MonoBehaviour
{
    public float endHere;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localPosition = new Vector3(0, -600, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition += new Vector3(0, 15, 0);
        if (this.transform.position.y >= endHere)
        {
            SceneManager.LoadScene("1STARTHERE");
        }

    }
}
