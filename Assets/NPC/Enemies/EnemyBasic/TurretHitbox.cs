using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHitbox : MonoBehaviour
{
    public BaseEnemyStats baseEnemyStats;

    public void OnTriggerEnter(Collider collider) {
        Debug.Log(collider);
        //These two tags represent that this entity may be damaged by both player or enemy hitboxes
        if (collider.tag == "hitbox" || collider.tag == "EnemyHitbox") {
            //Getting the data from the damaging hitbox
            HitboxStats hbs = collider.gameObject.GetComponent<HitboxStats>();
            //Play hitting sound
            hbs.PlayHitSounds();
            //Now receive the damage from the hitbox data
            baseEnemyStats.ReceiveDamage(hbs.damage);

            //FInally check if it can receive knockback, and the direction of it
            if (baseEnemyStats.canReceiveKnockback) {
                if (hbs.knockbackDir != Vector3.zero)
                    baseEnemyStats.ReceiveKnockback(hbs.knockback, hbs.knockbackDir);
                else {
                    baseEnemyStats.ReceiveKnockback(hbs.knockback, transform.position - collider.transform.position);
                }
            }

        }

    }
}
