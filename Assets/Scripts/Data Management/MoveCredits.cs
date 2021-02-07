using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCredits : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.localPosition = new Vector3(0, -100, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition += new Vector3(0, 150 * Time.deltaTime, 0);
    }
}
