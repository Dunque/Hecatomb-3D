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
    Animator animator;

    void Awake()
    {
        currentHp = maxHp;
        body = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    public void ReceiveDamage(float damage)
    {
        currentHp -= damage;
        if (animator != null) {
            animator.Play($"Hit{Random.Range(0,2)}");

            if (currentHp <= 0f) {
                isDead = true;
                currentHp = 0f;
                if (animator != null) {
                    animator.SetLayerWeight(1, 0);
                    animator.SetInteger("DeadNumAnim", Random.Range(0, 2));
                    animator.SetBool("Dead", true);
                }

            }
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
