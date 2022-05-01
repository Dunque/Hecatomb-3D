using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{

    public Transform cam;

    [Header("Weapon Stats")]
    public float range;
    public float damage;
    public float knockback;

    public int maxAmmo;
    public int currentAmmo;

    public ShotTrails shotTrails;

    [Header("Animation")]
    public Sprite gunIcon;
    public string animName;

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

    public void AddAmmo(int amount)
    {
        if (currentAmmo + amount > maxAmmo)
            currentAmmo = maxAmmo;
        else
            currentAmmo = currentAmmo + amount;
    }

    public bool CanShoot()
    {
        return (currentAmmo > 0);
    }

    public abstract Vector3 GetShootingDir();
}
