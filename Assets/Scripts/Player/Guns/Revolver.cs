using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : Gun
{

    public override void Awake()
    {
        base.Awake();
        range = 50f;
        damage = 30f;
        knockback = 15f;
        maxAmmo = 24;
        animName = "Revolver";
    }

    public override void Shoot()
    {
        base.Shoot();
        currentAmmo--;

        RaycastHit hit;
        Vector3 shootingDir = GetShootingDir();

        if (Physics.Raycast(cam.position, shootingDir, out hit, range))
        {
            EntityStats entStats = null;
            if ((entStats = hit.collider.GetComponent<EntityStats>()) != null)
            {
                entStats.ReceiveDamage(damage);
                entStats.ReceiveKnockback(knockback, shootingDir);
            }
            CreateTrail(hit.point);
        }
        else
        {
            CreateTrail(cam.position + shootingDir * range);
        }
        
    }

    public override Vector3 GetShootingDir()
    {
        Vector3 targetPos = cam.position + cam.forward * range;
        Vector3 dir = targetPos - cam.position;
        return dir.normalized;
    }
}
