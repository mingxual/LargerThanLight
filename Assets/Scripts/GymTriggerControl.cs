using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymTriggerControl : MonoBehaviour
{
    [SerializeField] List<GameObject> triggers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTrigger(int index)
    {
        for(int i = 0; i < triggers.Count; i++)
        {
            triggers[i].SetActive(false);
        }

        triggers[index].SetActive(true);
    }
}
