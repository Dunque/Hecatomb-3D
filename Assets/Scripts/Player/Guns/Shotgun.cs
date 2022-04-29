using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [Header("Shot Spread")]
    [SerializeField] float spread = 6f;
    [SerializeField] int pelletsPerShell = 6;

    public override void Awake()
    {
        range = 50f;
        damage = 10f;
        knockback = 5f;
        maxAmmo = 20;
        animName = "Shotgun";
        base.Awake();
    }

    public override void Shoot()
    {
        base.Shoot();
        currentAmmo--;

        for (int i = 0; i < pelletsPerShell ; i++)
        {
            RaycastHit hit;
            Vector3 shootingDir = GetShootingDir();

            if (Physics.SphereCast(cam.position, 0.1f, shootingDir, out hit, range))
            {
                EntityStats entStats;
                if ((entStats = hit.collider.GetComponent<EntityStats>()) != null)
                {
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
    }

    public override Vector3 GetShootingDir()
    {
        Vector3 targetPos = cam.position + cam.forward * range;
        targetPos = new Vector3(
            targetPos.x + Random.Range(-spread, spread),
            targetPos.y + Random.Range(-spread, spread),
            targetPos.z + Random.Range(-spread, spread)
            );

        Vector3 dir = targetPos - cam.position;
        return dir.normalized;
    }
}
