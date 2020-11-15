using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animateText : MonoBehaviour
{

    public Texture2D texture1;
    public Texture2D texture2;
    public Texture2D texture3;
    public Texture2D texture4;
    public Texture2D texture5;
    public Texture2D texture6;
    public Texture2D texture7;
    public Texture2D texture8;
    public Texture2D texture9;
    public Texture2D texture10;
    public Texture2D texture11;
    public Texture2D texture12;
    public Texture2D texture13;
    public Texture2D texture14;
    public Texture2D texture15;
    public Texture2D texture16;
    public Texture2D texture17;
    public Texture2D texture18;
    public Texture2D texture19;
    public Texture2D texture20;
    public Texture2D texture21;
    public Texture2D texture22;
    public Texture2D texture23;
    public Texture2D texture24;
    public Texture2D texture25;
    public Texture2D texture26;
    public Texture2D texture27;
    public Texture2D texture28;
    public Texture2D texture29;
    public Texture2D texture30;
    public Texture2D texture31;
    public Texture2D texture32;
    public Texture2D texture33;
    public Texture2D texture34;
    public Texture2D texture35;
    public Texture2D texture36;
    public Texture2D texture37;
    public Texture2D texture38;
    public Texture2D texture39;
    public Texture2D texture40;
    public Texture2D texture41;
    public Texture2D texture42;
    public Texture2D texture43;
    public Texture2D texture44;
    public Texture2D texture45;
    public Texture2D texture46;
    public Texture2D texture47;
    public Texture2D texture48;
    public Texture2D texture49;
    public Texture2D texture50;



    public Renderer renderer;
    public bool change = true;

    public float winkTime;


    void Start()
    {
        changeTexture();
        renderer = GetComponent<Renderer>();
        StartCoroutine(changeTexture());

    }
    void Update()
    {
    }
    IEnumerator changeTexture()
    {
        while (change)
        {
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture1;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture2;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture3;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture4;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture5;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture6;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture7;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture8;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture9;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture10;
             yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture11;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture12;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture13;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture14;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture15;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture16;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture17;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture18;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture19;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture20;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture21;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture22;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture23;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture24;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture25;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture26;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture27;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture28;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture29;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture30;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture31;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture32;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture33;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture34;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture35;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture36;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture37;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture38;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture39;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture40;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture41;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture42;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture43;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture44;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture45;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture46;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture47;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture48;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture49;
            yield return new WaitForSeconds(winkTime);
            renderer.material.mainTexture = texture50;

        }
    }


}
