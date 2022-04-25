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

    public ShotTrails shotTrails;

    [Header("Animation")]
    public Sprite gunIcon;
    [SerializeField] public string animName;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        Camera camera = GetComponentInParent<Camera>();
        shotTrails = GetComponentInParent<ShotTrails>();
        cam = camera.transform;
        currentAmmo = maxAmmo;
    }

    public virtual void Shoot()
    {
        shotTrails.MuzzleExplosion();
    }

    public virtual void StopShoot()
    {

    }

    public bool CanShoot()
    {
        return (currentAmmo > 0);
    }

    public abstract Vector3 GetShootingDir();
}
