using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public List<GameObject> cameraPoses;
    public GameObject originCamera;
    public float transitionTime;
    public List<GameObject> cameraWalls;
    public float duration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        LeanTween.move(originCamera, cameraPoses[0].transform.position, transitionTime).setOnComplete(FadeCameraWalls);
    }

    public void ChangeToCamera(int index)
    {
        if(index >= 0)
        {
            LeanTween.move(originCamera, cameraPoses[index].transform.position, transitionTime);
        }
    }

    public void FadeCameraWalls()
    {
        foreach (GameObject go in cameraWalls)
        {
            StartCoroutine(FadeTo(go.GetComponent<MeshRenderer>().material, 0f, duration));
            // Color temp = go.GetComponent<MeshRenderer>().material.color;
            // temp.a = 0;
            // go.GetComponent<MeshRenderer>().material.color = temp;
            // I cannot get the walls to fade out properly
            // the banners go invisible in front of the material
            // go.SetActive(false);
        }
    }

    IEnumerator FadeTo(Material material, float targetOpacity, float duration)
    {
        Color color = material.color;
        float startOpacity = color.a;

        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            float blend = Mathf.Clamp01(t / duration);

            color.a = Mathf.Lerp(startOpacity, targetOpacity, blend);

            material.color = color;

            yield return new WaitForEndOfFrame();
        }
    }
}
