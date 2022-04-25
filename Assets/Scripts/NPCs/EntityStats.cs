using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float maxHp = 50f;
    [SerializeField] public float currentHp;
    [SerializeField] public bool canReceiveKnockback;
    public Rigidbody body;
    public bool isDead;

    public virtual void Awake()
    {
        currentHp = maxHp;
        body = GetComponent<Rigidbody>();
    }

    public virtual void ReceiveDamage(float damage)
    {
        currentHp -= damage;
        if (currentHp <= 0f)
        {
            isDead = true;
            currentHp = 0f;
        }    
    }

    public virtual void ReceiveHealing(float heal)
    {
        if (currentHp + heal >= maxHp)
            currentHp = maxHp;
        else
            currentHp = currentHp + heal;
    }

    public virtual void ReceiveKnockback(float magnitude, Vector3 dir)
    {
        body.velocity += magnitude * dir;
    }

    //Physical colliders, like melee weapons or explosions
    public virtual void OnTriggerEnter(Collider collider)
    {
        //These two tags represent that this entity may be damaged by both player or enemy hitboxes
        if (collider.tag == "hitbox" || collider.tag == "EnemyHitbox")
        {
            //Getting the data from the damaging hitbox
            HitboxStats hbs = collider.gameObject.GetComponent<HitboxStats>();

            ReceiveDamage(hbs.damage);

            if (canReceiveKnockback) {
                if (hbs.knockbackDir != Vector3.zero)
                    ReceiveKnockback(hbs.knockback, hbs.knockbackDir);
                else {
                    ReceiveKnockback(hbs.knockback, transform.position - collider.transform.position);
                }
            }
            
        }

    }

}
