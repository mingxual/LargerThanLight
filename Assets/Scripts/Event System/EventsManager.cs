using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class EventsManager : MonoBehaviour
{
    // Singleton class
    public static EventsManager instance;

    public List<string> m_EventsMapKeys;
    public List<UnityEvent> m_EventsMapValues;

    private Dictionary<string, UnityEvent> m_EventsMap;

    //Data for dashboard
    int subLevel = 1;
    int sceneIndex = 0;
    float lastRecord = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Collect the keys and values entered and put them into the dictionary
        m_EventsMap = new Dictionary<string, UnityEvent>();

        int size_Keys = m_EventsMapKeys.Count;
        int size_Values = m_EventsMapValues.Count;

        if(size_Keys != size_Values)
        {
            Debug.LogError("Sizes of keys and values in EventsManager are not equal");
            return;
        }

        for (int i = 0; i < size_Keys; ++i)
        {
            m_EventsMap.Add(m_EventsMapKeys[i], m_EventsMapValues[i]);
        }
    }

    public void InvokeEvent(string key)
    {
        if(m_EventsMap.ContainsKey(key))
        {
            if (sceneIndex != SceneManager.GetActiveScene().buildIndex)
            {
                subLevel = 1;
                lastRecord = 0;
                sceneIndex = SceneManager.GetActiveScene().buildIndex;
            }
            
            Debug.Log("Event Invoke:" + key);
            m_EventsMap[key].Invoke();

            if (key == "SkiaShadowTouch")
            {
                return;
            }

            string curTime = EventTime.GetTime(DateTime.Now);          
            float sceneTime = Time.timeSinceLevelLoad;
            float timeSpent = sceneTime - lastRecord;

            string record = curTime + " " + 
                SceneManager.GetActiveScene().name + ":" +
                "subLevel:" + subLevel + 
                " time:" + timeSpent.ToString() +
                " sceneTime:" + Time.timeSinceLevelLoad + 
                " TotalTime: " + Time.time;


            //Debug.Log(record);

            AnalyticsResult ar = Analytics.CustomEvent(SceneManager.GetActiveScene().name, new Dictionary<string, object> {
                {"subLevel",subLevel},
                {"Time",timeSpent}
            });
            

            //Debug.Log("Analytics Result:"+ar.ToString());
            
            subLevel++;
            lastRecord = sceneTime;
            
            // m_EventsMap.Remove(key);
        }
        else
        {
            Debug.LogError(key + "is not set in EventsManager");
        }
    }
}
