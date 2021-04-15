using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShadowEntity : SCObstacle
{
    public bool projActive;
    public GameObject projPrefab;
    public float fireCooldown;

    private float fireCooldownTimer;

    public List<Transform> targetTransforms;
    public SimpleController skia;
    public GameObject lux;

    public GameObject aura;
    public LightFlicking lf;

    private bool checkForLux;
    private bool luxTrigger;
    private bool luxFlickering;

    public AudioClip sound1;
    public AudioSource otherSource;

    public void Start()
    {
        otherSource = this.gameObject.GetComponent<AudioSource>();
    }

    public void FireProjectiles()
    {
        GameObject projGO;
        for(int i = 0; i < 3; i++)
        {
            otherSource.clip = sound1;
            otherSource.Play();
            projGO = Instantiate(projPrefab, projPrefab.transform.position, Quaternion.identity);
            projGO.SetActive(true);
            projGO.transform.Rotate(0, 0, 15 * (i - 1));
            projGO.GetComponent<ShadowEntityProjectile>().se = this;
        }
        AudioManager.instance.PlayOnceNoPlace("Projectile_Spwan");

        fireCooldownTimer = fireCooldown;
    }

    private void Update()
    {
        if (!projActive) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            if(Input.GetKey(KeyCode.LeftShift))
                FireProjectiles();

        if (fireCooldownTimer > 0)
        {
            fireCooldownTimer -= Time.deltaTime;
            return;
        }

        if(TrackSkiaTargets())
        {
            FireProjectiles();
            checkForLux = true;
        }

        aura.SetActive(SCManager.Instance.ProjectileCount() > 0);

        if (checkForLux)
        {
            if (SCManager.Instance.ProjectileCount() == 0)
            {
                checkForLux = false;
            }
            else
            {
                if (!LuxInAura())
                {
                    lf.turnOffFlicker();
                    luxTrigger = false;
                }
                else if (!luxTrigger)
                {
                    print("lux trigger initiated");
                    luxTrigger = true;
                    Invoke("LuxTriggerActivate", 2f);
                }
            }
        }
    }

    private bool LuxInAura()
    {
        return Vector3.Distance(lux.transform.position, aura.transform.position) < 18;
    }

    private void LuxTriggerActivate()
    {
        if (!LuxInAura())
        {
            print("lux left area before trigger");
            return;
        }
        print("lux trigger activated");
        lf.turnOnFlicker();
        Invoke("LuxKill", 20f);
    }

    private void LuxKill()
    {
        if(!LuxInAura())
        {
            print("lux left area before kill");
            return;
        }
        print("kill");
        lf.turnOffLight();
        skia.Disable();
        lux.GetComponent<LightController>().enabled = false;
        Invoke("Restart", 2f);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private bool TrackSkiaTargets()
    {
        foreach(Transform t in targetTransforms)
        {
            if (!t.gameObject.activeSelf)
                continue;

            if (Vector3.SqrMagnitude(t.transform.position - skia.GetWorldPosition3D()) < 36)
                return true;
        }
        return false;
    }

    private bool initialDestroy = false;

    public void DestroyedProjectile()
    {
        if (initialDestroy) return;

        initialDestroy = true;
        EventsManager.instance.InvokeEvent("initialDestroy");
    }
}
