using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicking : MonoBehaviour
{
    [SerializeField] private new Light light;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;

    [Tooltip("How often the light changes its intensity")]
    public float subphasePeriod;
    
    [Tooltip("How long the flicking action plays")]
    public float flickingPeriod;

    [Tooltip("How long there is a parse before the light starts to flick again")]
    public float parseInterval;

    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    public int smoothing = 5;

    // private timers to count for phase change
    private float timer;
    private float phasetimer;
    private float currPeriod;
    
    // Bool to track whether the flicker effect is on
    public bool flickerOn = false;
    // private bool to track whether the flicker
    // private bool flickerOnLastFrame = false;

    // Variables to accumulate the randomness of the light intensity
    Queue<float> smoothQueue;
    float lastSum = 0;

    // Start is called before the first frame update
    void Start()
    {
        light.intensity = maxIntensity;
        smoothQueue = new Queue<float>();

        if(flickerOn)
        {
            turnOnFlicker();
        }
    }

    private void Reset()
    {
        smoothQueue.Clear();
        lastSum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(flickerOn)
        {
            // Change the light intensity only when subphasePeriod passes
            if (currPeriod == flickingPeriod)
            {
                phasetimer += Time.deltaTime;
                if (phasetimer >= subphasePeriod)
                {
                    ChangeFlickingSubPhase();
                    phasetimer = 0.0f;
                }
            }

            timer += Time.deltaTime;
            
            // Check the value of timer and change the behavior accordingly 
            if(currPeriod == flickingPeriod && timer >= flickingPeriod)
            {
                currPeriod = parseInterval;
                light.intensity = maxIntensity;
                timer = 0.0f;
            }
            else if(currPeriod == parseInterval && timer >= parseInterval)
            {
                currPeriod = flickingPeriod;
                Reset();
                timer = 0.0f;
            }
        }

        // flickerOnLastFrame = flickerOn;
    }

    void ChangeFlickingSubPhase()
    {
        // pop off an item if too big
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(minIntensity, maxIntensity);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        // Calculate new smoothed average
        float smoothed_average = lastSum / (float)smoothQueue.Count;
        light.intensity = smoothed_average > maxIntensity ? maxIntensity : smoothed_average;
    }

    // Turn on the flicker effect
    public void turnOnFlicker()
    {
        flickerOn = true;
        timer = 0.0f;
        phasetimer = 0.0f;
        currPeriod = flickingPeriod;
    }

    // Turn off the flicker effect
    public void turnOffFlicker()
    {
        flickerOn = false;
        light.intensity = maxIntensity;
    }

    // Turn on the light
    public void turnOnLight()
    {
        light.enabled = true;
    }

    // Turn off the light
    public void turnOffLight()
    {
        light.enabled = false;
    }
}
