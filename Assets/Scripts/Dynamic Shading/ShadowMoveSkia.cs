using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMoveSkia : MonoBehaviour
{
    float ratio;
    int vertexX;
    int vertexY;
    EdgeCollider2D edgeCollider;
    Vector2[] points;

    // Start is called before the first frame update
    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        points = edgeCollider.points;
        //ratio = -1;
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    public Vector2  CalulateSkiaDisplacement(Vector2 point, float ratioInput)
    {
        //GetLines(point);
        Vector2 position = points[vertexX] + (points[vertexY] - points[vertexX]) * ratio;
        return position;
    }

    public void GetLines(Vector2 point)
    {
        bool stopLoop = false;
        for (int i = 0; i < points.Length; i++)
        {
            int j = i + 1;
            if (i == (points.Length-1))
            {
                j = 0;
            }
            while((points[i] - points[j]) == Vector2.zero)
            {
                j += 1;
                if(j >= points.Length)
                {
                    stopLoop = true;
                    j = 0;
                }
            }

            if(Vector3.Cross((point - points[i]),(points[i] - points[j])).magnitude <= 0.05f)
            {
                vertexX = i;
                vertexY = j;
                break;
            }

            i = j - 1;
            if (stopLoop)
            {
                break;
            }
        }
    }

    public float UpdateRatio(Vector2 point)
    {
        GetLines(point);
        ratio = (point - points[vertexX]).magnitude / (points[vertexY] - points[vertexX]).magnitude;
        return ratio;
    }
}
