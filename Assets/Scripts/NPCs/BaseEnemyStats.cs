using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyStats : EntityStats
{
    [Header("Stats")]
    public List<Rigidbody> ragdollBody;
    Animator animator;

    public override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody>();
        ragdollBody = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        animator = GetComponentInChildren<Animator>();
    }

    public override void ReceiveDamage(float damage)
    {
        currentHp -= damage;
        if (animator != null)
        {
            animator.Play($"Hit{Random.Range(0, 2)}");

            if (currentHp <= 0f)
            {
                isDead = true;
                currentHp = 0f;
                if (animator != null)
                {
                    animator.SetInteger("DeadNumAnim", Random.Range(0, 2));
                    animator.SetBool("Dead", true);
                }

            }
        }
    }

    public override void ReceiveKnockback(float magnitude, Vector3 dir)
    {
        if (isDead)
            if (ragdollBody != null)
                foreach (Rigidbody rb in ragdollBody)
                    rb.velocity += magnitude * dir;
            else
                body.velocity += magnitude * dir;
        else
            body.velocity += magnitude * dir;
    }
}
