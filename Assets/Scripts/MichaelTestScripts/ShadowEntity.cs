using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowEntity : SCObstacle
{
    public bool projActive;
    public GameObject projPrefab;
    public float fireCooldown;

    private float fireCooldownTimer;

    public List<Transform> targetTransforms;
    public SimpleController skia;

    void CollisionTrigger(GameObject skia)
    {
        skia.GetComponent<SimpleController>().ResetSkia();
    }

    private void FireProjectiles()
    {
        GameObject projGO;
        for(int i = 0; i < 3; i++)
        {
            projGO = Instantiate(projPrefab, projPrefab.transform.position, Quaternion.identity);
            projGO.SetActive(true);
            projGO.transform.Rotate(0, 0, 15 * (i - 1));
        }

        fireCooldownTimer = fireCooldown;
    }

    private void Update()
    {
        if (!projActive) return;

        if (fireCooldownTimer > 0)
        {
            fireCooldownTimer -= Time.deltaTime;
            return;
        }

        if(TrackSkiaTargets())
        {
            FireProjectiles();
        }
    }

    private bool TrackSkiaTargets()
    {
        foreach(Transform t in targetTransforms)
        {
            if (Vector3.SqrMagnitude(t.transform.position - skia.GetWorldPosition()) < 36)
                return true;
        }
        return false;
    }
}
