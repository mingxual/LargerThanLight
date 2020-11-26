using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMoveSkia : MonoBehaviour
{
    float ratio;
    int vertexX;
    int vertexY;
    float left;
    float right;
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

    public float  CalulateSkiaDisplacement(Vector2 point)
    {
        GetLines(point);
        //Vector2 position = points[vertexX] + (points[vertexY] - points[vertexX]) * ratio;
        float displacement = left + (right - left) * ratio;
        Debug.DrawLine(points[vertexX], points[vertexY], Color.white);
        return displacement;
    }

    public void GetLines(Vector2 point)
    {
        /*
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
        }*/

        points = edgeCollider.points;
        Vector2 leftMost = new Vector2(float.MaxValue, 0);
        Vector2 rightMost = new Vector2(float.MinValue, 0);
        for(int i = 0; i < points.Length; i++)
        {
            if (points[i].x < leftMost.x)
            {
                leftMost = points[i];
                vertexX = i;
            }

            if(points[i].x > rightMost.x)
            {
                rightMost = points[i];
                vertexY = i;
            }
        }

        left = leftMost.x;
        right = rightMost.x;
    }

    public float UpdateRatio(Vector2 point)
    {
        GetLines(point);
        //ratio = (point - points[vertexX]).magnitude / (points[vertexY] - points[vertexX]).magnitude;
        ratio = (point.x - left) / (right - left);
        return ratio;
    }
}
