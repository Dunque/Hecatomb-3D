using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    public HealthBar hbar;

    public override void Awake()
    {
        base.Awake();
        hbar = GetComponentInChildren<HealthBar>();
        hbar.SetMaxHealth(maxHp);
    }

    public override void ReceiveDamage(float damage)
    {
        currentHp -= damage;
        hbar.SetHealth(currentHp);

        if (currentHp <= 0f)
        {
            isDead = true;
            currentHp = 0f;
        }
    }

    //Physical colliders, like melee weapons or explosions
    public override void OnTriggerEnter(Collider collider)
    {
        //only damaged by enemy hitboxes, in order to not collide with self hitboxes
        if (collider.tag == "EnemyHitbox")
        {
            //Getting the data from the damaging hitbox
            HitboxStats hbs = collider.gameObject.GetComponent<HitboxStats>();

            ReceiveDamage(hbs.damage);

            if (canReceiveKnockback)
            {
                if (hbs.knockbackDir != Vector3.zero)
                    ReceiveKnockback(hbs.knockback, hbs.knockbackDir);
                else
                {
                    ReceiveKnockback(hbs.knockback, transform.position - collider.transform.position);
                }
            }
        }
    }
}
