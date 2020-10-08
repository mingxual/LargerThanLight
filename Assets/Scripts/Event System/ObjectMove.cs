using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ObjectMove : MonoBehaviour
{
    public Axis m_direction;
    public float m_MoveTargetPos;
    public float m_TimeDuration;
    public GameManager m_GameManager;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToTargetPos()
    {
        if(m_direction == Axis.X)
        {
            LeanTween.moveX(gameObject, m_MoveTargetPos, m_TimeDuration);
        }
        else if(m_direction == Axis.Y)
        {
            LeanTween.moveY(gameObject, m_MoveTargetPos, m_TimeDuration);
        }
        else if(m_direction == Axis.Z)
        {
            LeanTween.moveZ(gameObject, m_MoveTargetPos, m_TimeDuration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        m_GameManager.EnableCharacterControl();
        Debug.Log("enable character control");
    }

    public void RotateToTargetPos()
    {
        if (m_direction == Axis.X)
        {
            LeanTween.rotateX(gameObject, m_MoveTargetPos, m_TimeDuration);
        }
        else if (m_direction == Axis.Y)
        {
            LeanTween.rotateY(gameObject, m_MoveTargetPos, m_TimeDuration);
        }
        else if (m_direction == Axis.Z)
        {
            LeanTween.rotateZ(gameObject, m_MoveTargetPos, m_TimeDuration);
        }
    }
}
