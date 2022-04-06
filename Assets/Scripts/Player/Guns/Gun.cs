using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{

    public Transform cam;

    [Header("Weapon Stats")]
    [SerializeField] public float range;
    [SerializeField] public float damage;
    [SerializeField] public float knockback;

    [SerializeField] public int maxAmmo;
    public int currentAmmo;

    [Header("Bullet tray")]
    [SerializeField] GameObject bulletTrail;
    [SerializeField] Transform muzzle;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] float fadeDuration = 5f;

    [Header("Animation")]
    [SerializeField] public string animName;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        Camera camera = GetComponentInParent<Camera>();
        cam = camera.transform;
        currentAmmo = maxAmmo;
    }

    public virtual void Shoot()
    {
        muzzleFlash.Play();
    }

    public bool CanShoot()
    {
        return (currentAmmo > 0);
    }

    public abstract Vector3 GetShootingDir();

    //This function instantiates a bullet trail coming from the muzzle of the 
    //weapon, and it calls the fading coroutine for that trail.
    public void CreateTrail(Vector3 end)
    {
        GameObject bt = Instantiate(bulletTrail);
        LineRenderer lr = bt.GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { muzzle.position, end });
        StartCoroutine(FadeTrail(bt, lr));
    }

    //This function reduces the opacity of the line renderer, until it's invisible.
    //Then, it destroys it. For that purpose, we need to pass the object reference,
    //not only the line renderer.
    IEnumerator FadeTrail(GameObject bt, LineRenderer lr)
    {
        float alphaEnd = 1;
        float alphaStart = 0.3f;
        while (alphaEnd > 0)
        {
            alphaStart -= Time.deltaTime / fadeDuration;
            alphaEnd -= Time.deltaTime / fadeDuration;
            lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alphaStart);
            lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, alphaEnd);
            yield return null;
        }
        Destroy(bt);
    }
}
