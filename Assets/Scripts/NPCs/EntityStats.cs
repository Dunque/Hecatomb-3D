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
    public float iframesTime = 0.2f;
    public bool isDead;

    protected bool damageable = true;

    public virtual void Awake()
    {
        currentHp = maxHp;
        body = GetComponent<Rigidbody>();
    }

    //Timer used to prevent being damaged a lot of times in the span of a few frames
    IEnumerator IframesTimer()
    {
        damageable = false;
        float timer = 0;

        while (timer < iframesTime)
        {
            timer += Time.deltaTime / iframesTime;
            yield return null;
        }
        damageable = true;
    }

    public virtual void ReceiveDamage(float damage)
    {
        if (damageable)
        {
            currentHp -= damage;
            if (currentHp <= 0f)
            {
                currentHp = 0f;
                if (!isDead)
                    Die();
                isDead = true;
            }
        }

    }

    public virtual void ReceiveHealing(float heal)
    {
        if (currentHp + heal >= maxHp)
            currentHp = maxHp;
        else
            currentHp += heal;
    }

    public virtual void ReceiveKnockback(float magnitude, Vector3 dir)
    {
        if (damageable)
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
            //Play hitting sound
            hbs.PlayHitSounds();

            //Now receive the damage from the hitbox data
            ReceiveDamage(hbs.damage);

            //FInally check if it can receive knockback, and the direction of it
            if (canReceiveKnockback) {
                if (hbs.knockbackDir != Vector3.zero)
                    ReceiveKnockback(hbs.knockback, hbs.knockbackDir);
                else {
                    ReceiveKnockback(hbs.knockback, transform.position - collider.transform.position);
                }
            }
            
        }

    }

    public virtual void Die()
    {

    }

}
