using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyStats : EntityStats
{
    [Header("Stats")]
    public List<Rigidbody> ragdollBody;
    Animator animator;

    EnemyWeaponSlotManager weaponSlotManager;
    RagdollEnemy ragdollEnemy;


    [Header("Damage color")]
    public Renderer[] meshesRenderer;
    public float blinkIntensity = 3;
    public float blinkDuration = 0.6f;
    float blinkTimer;

    public int numHitAnims = 1;
    
    public override void Awake()
    {
        base.Awake();
        body = GetComponent<Rigidbody>();
        ragdollBody = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>());
        animator = GetComponentInChildren<Animator>();
        weaponSlotManager = GetComponentInChildren<EnemyWeaponSlotManager>();
        ragdollEnemy = GetComponent<RagdollEnemy>();
    }

    private void Update() {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        if (intensity > 1.0f) {
            foreach(Renderer mesh in meshesRenderer) {
                mesh.material.color = Color.red * intensity;
            }
        } else {
            foreach(Renderer mesh in meshesRenderer) {
                mesh.material.color = Color.white * intensity;
            }
        }
    }

    public override void ReceiveDamage(float damage)
    {
        currentHp -= damage;

        blinkTimer = blinkDuration;
        if (animator != null)
        {
            animator.CrossFade($"Hit{Random.Range(0, numHitAnims)}",0.2f);

            if (currentHp <= 0f)
            {
                isDead = true;
                currentHp = 0f;
                Die();
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

    public override void Die() {
        if (weaponSlotManager != null) {
            weaponSlotManager.CloseColliders();
        }
        if(ragdollEnemy != null) {
            ragdollEnemy.Die();
        }
    }
}
