using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEntityProjectile : MonoBehaviour
{
    private void OnEnable()
    {
        SCManager.Instance.AddShadowProj(this);
    }

    private void OnDisable()
    {
        SCManager.Instance.RemoveShadowProj(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hello");
        SCEventHandle handle = collision.GetComponent<SCEventHandle>();
        if (handle)
        {
            if(handle.corrObject)
            {
                SCObstacle obs = handle.corrObject.GetComponent<SCObstacle>();
                if (!obs) return;

                if (obs.shadowprojaffect)
                {
                    print("hit object");
                    obs.HitByShadowProj();
                    Destroy(gameObject);
                }
            }
        }
    }

    public Vector3 GetWorldPosition()
    {
        RaycastHit hitInfo;
        bool hitWall2D = Physics.Raycast(transform.position, Vector3.forward, out hitInfo, 100f, SCManager.Instance.m_Wall2DLayerMask, QueryTriggerInteraction.Collide);
        if (hitWall2D)
        {
            Wall2D wall2D = hitInfo.transform.GetComponent<Wall2D>();
            return wall2D.SwitchTo3D(hitInfo.transform.InverseTransformPoint(hitInfo.point));
        }
        return transform.position;
    }
}
