using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDamage : MonoBehaviour
{
    public BaseEnemyStats baseEnemyStats;
    //Physical colliders, like melee weapons or explosions
    public void OnTriggerEnter(Collider collider) {
        //only damaged by enemy hitboxes, in order to not collide with self hitboxes
        if (collider.tag == "hitbox") {
            //Getting the data from the damaging hitbox
            HitboxStats hbs = collider.gameObject.GetComponent<HitboxStats>();
            
            if (baseEnemyStats.canReceiveKnockback) {
                if (hbs.knockbackDir != Vector3.zero)
                    baseEnemyStats.ReceiveKnockback(hbs.knockback, hbs.knockbackDir);
                else {
                    baseEnemyStats.ReceiveKnockback(hbs.knockback, transform.position - collider.transform.position);
                }
            }

            baseEnemyStats.ReceiveDamage(hbs.damage);
        }
    }
}
