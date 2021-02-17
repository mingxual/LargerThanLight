﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEntityProjectile : MonoBehaviour
{
    private SimpleController skia;

    private void OnEnable()
    {
        SCManager.Instance.AddShadowProj(this);
        skia = SCManager.Instance.GetSkia();
    }

    private void OnDisable()
    {
        SCManager.Instance.RemoveShadowProj(this);
    }

    [SerializeField] float stallTimer = 2f;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float rotateSpeed = 1f;

    private void Update()
    {
        if(stallTimer > 0)
        {
            stallTimer -= Time.deltaTime;
            transform.position += transform.up * 2 * moveSpeed * Time.deltaTime;
            return;
        }

        transform.position += transform.up * moveSpeed * Time.deltaTime;
        float dotp = Vector3.Dot((skia.transform.position - transform.position).normalized, transform.right);
        transform.Rotate(0, 0, -rotateSpeed * dotp * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("hello");
        SCEventHandle handle = collision.GetComponent<SCEventHandle>();
        if (handle)
        {
            if(handle.corrObject)
            {
                SCObstacle obs = handle.corrObject.GetComponent<SCObstacle>();
                if (!obs) return;

                if (obs.shadowprojaffect)
                {
                    //print("hit object");
                    obs.HitByShadowProj();
                    Destroy(gameObject);
                }
            }
            return;
        }

        SimpleController sc = collision.GetComponent<SimpleController>();
        if(sc)
        {
            sc.ResetSkia();
            Destroy(gameObject);
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