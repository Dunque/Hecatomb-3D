using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Gun {
    [Header("Shot Spread")]
    public float spread = 6f;
    [SerializeField] int pelletsPerShell = 6;

    public Transform raycast;

    public Light warningLight;

    public override void Awake() {
        range = 50f;
        damage = 15f;
        knockback = 10f;
        animName = "Turret";
        base.Awake();
    }

    public override void Shoot() {
        base.Shoot();

        for (int i = 0; i < pelletsPerShell; i++) {
            RaycastHit hit;
            
            Vector3 startPosition = raycast.position;
            Vector3 shootingDir = raycast.forward;
            
            if (Physics.SphereCast(startPosition, 0.1f, shootingDir, out hit, range, ~ignoreLayers)) {
                EntityStats entStats;
                if ((entStats = hit.transform.GetComponent<EntityStats>()) != null) {
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
        throw new System.NotImplementedException();
    }
}
