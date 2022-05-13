using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Gun
{

    public override void Awake()
    {
        range = 50f;
        damage = 30f;
        knockback = 15f;
        maxAmmo = 24;
        animName = "Revolver";
        base.Awake();
    }

    public override void Shoot()
    {
        base.Shoot();
        currentAmmo--;

        RaycastHit hit;
        Vector3 shootingDir = GetShootingDir();
        if (Physics.SphereCast(cam.position, 0.1f, shootingDir, out hit, range, ~ignoreLayers))
        {
            EntityStats entStats;
            entStats = hit.transform.GetComponent<EntityStats>();
            if (entStats == null) {
                entStats = hit.transform.GetComponentInParent<EntityStats>();
            }
            if (entStats == null) {
                entStats = hit.transform.GetComponentInChildren<EntityStats>();
            }
            if (entStats != null) {
                entStats.ReceiveDamage(damage);
                entStats.ReceiveKnockback(knockback, shootingDir);
                entStats.ReceiveDamage(damage);
            }
            shotTrails.CreateTrail(hit.point);
        }
        else
        {
            shotTrails.CreateTrail(cam.position + shootingDir * range);
        }

    }

    public override Vector3 GetShootingDir()
    {
        Vector3 targetPos = cam.position + cam.forward * range;
        Vector3 dir = targetPos - cam.position;
        return dir.normalized;
    }
}
