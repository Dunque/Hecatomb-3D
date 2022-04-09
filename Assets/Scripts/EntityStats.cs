using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float maxHp = 50f;
    [SerializeField] public float currentHp;
    public Rigidbody body;
    public bool isDead;

    void Awake()
    {
        currentHp = maxHp;
        body = GetComponent<Rigidbody>();
    }

    public void ReceiveDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0f)
        {
            isDead = true;
        }
    }

    public void ReceiveKnockback(float magnitude, Vector3 dir)
    {
        body.velocity += magnitude * dir;
    }

    //Physical colliders, like melee weapons or explosions
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "hitbox")
        {
            //Getting the data from the damaging hitbox
            HitboxStats hbs = collider.gameObject.GetComponent<HitboxStats>();

            ReceiveDamage(hbs.damage);

            if (hbs.knockbackDir != Vector3.zero)
                ReceiveKnockback(hbs.knockback, hbs.knockbackDir);
            else
            {
                ReceiveKnockback(hbs.knockback, transform.position - collider.transform.position);
            }
        }

    }
}
