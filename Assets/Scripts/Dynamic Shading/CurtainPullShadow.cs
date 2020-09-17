using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainPullShadow : MonoBehaviour
{
    // The matching curtain pull shadow on 2d wall
    public GameObject m_Shadow;
    // Its transform
    private Transform m_ShadowTrans;
    // Its matching collider2d
    private BoxCollider2D m_ShadowCollider;

    public GameObject m_LightSource;
    public LayerMask wallLayerMask;

    private Vector3 m_CenterPos;
    private Vector3 m_BottomLeftPos;
    private Vector3 m_BottomRightPos;

    // Start is called before the first frame update
    void Start()
    {
        if(m_Shadow == null)
        {
            Debug.LogError("Curtain pull does not have matching shadow pull");
            return;
        }

        m_ShadowTrans = m_Shadow.GetComponent<Transform>();
        m_ShadowCollider = m_Shadow.GetComponent<BoxCollider2D>();

        m_CenterPos = transform.position;

        m_BottomLeftPos = transform.position;
        m_BottomLeftPos.y -= transform.localScale.y;
        m_BottomLeftPos.x -= transform.localScale.x;

        m_BottomRightPos = transform.position;
        m_BottomRightPos.y += transform.localScale.y;
        m_BottomRightPos.x += transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = m_CenterPos - m_LightSource.transform.position;
        dir = dir.normalized;
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, dir, out hitInfo, 1000.0f, wallLayerMask, QueryTriggerInteraction.Collide))
        {
            Wall3D wall3D = hitInfo.collider.gameObject.GetComponent<Wall3D>();
            Vector3 point = wall3D.RaycastToWall2D(hitInfo.collider.gameObject.transform.InverseTransformPoint(hitInfo.point), transform.position);
            Vector2 point2D = Vector2.right * point.x + Vector2.up * point.y;

            m_ShadowTrans.position = new Vector3(point2D.x, point2D.y, 0);
        }


    }

    public void OnTriggerEnter(Collider other)
    {
           
    }
}
