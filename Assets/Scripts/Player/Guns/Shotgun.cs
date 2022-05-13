using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun {
    [Header("Shot Spread")]
    [SerializeField] float spread = 6f;
    [SerializeField] int pelletsPerShell = 6;

    public Transform raycast;

    public override void Awake() {
        range = 50f;
        damage = 10f;
        knockback = 10f;
        maxAmmo = 20;
        animName = "Shotgun";
        base.Awake();
    }

    public override void Shoot() {
        base.Shoot();
        currentAmmo--;

        for (int i = 0; i < pelletsPerShell; i++) {
            RaycastHit hit;

            Vector3 shootingDir;

            Vector3 startPosition;
            if (cam != null) {
                startPosition = cam.position;
                shootingDir = GetShootingDir();
            } else {
                startPosition = raycast.position;
                shootingDir = raycast.forward;
            }
            
            if (Physics.SphereCast(startPosition, 0.1f, shootingDir, out hit, range, ~ignoreLayers)) {
                EntityStats entStats;
                entStats = hit.transform.GetComponent<EntityStats>();
                if (entStats == null) {
                    entStats = hit.transform.GetComponentInParent<EntityStats>();
                }
                if (entStats == null) {
                    entStats = hit.transform.GetComponentInChildren<EntityStats>();
                }
                if (entStats != null) {
                    Debug.Log(damage);
                    entStats.ReceiveDamage(damage);
                    if (entStats.canReceiveKnockback) {
                        entStats.ReceiveKnockback(knockback, shootingDir);
                    }
                }
                shotTrails.CreateTrail(hit.point);
            } else {
                shotTrails.CreateTrail(startPosition + shootingDir * range);
            }
        }
    }

    public override Vector3 GetShootingDir() {
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
