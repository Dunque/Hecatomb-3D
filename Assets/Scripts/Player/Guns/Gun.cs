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

    public void CreateTrail(Vector3 end)
    {
        LineRenderer lr = Instantiate(bulletTrail).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { muzzle.position, end });
        StartCoroutine(FadeTrail(lr));
    }

    IEnumerator FadeTrail(LineRenderer lr)
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
        Destroy(lr);
    }

}
