using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    // Singleton class
    public static EventsManager instance;

    public List<string> m_EventsMapKeys;
    public List<UnityEvent> m_EventsMapValues;

    private Dictionary<string, UnityEvent> m_EventsMap;

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
            m_EventsMap[key].Invoke();
            // m_EventsMap.Remove(key);
        }
        else
        {
            Debug.LogError(key + "is not set in EventsManager");
        }
    }
}
