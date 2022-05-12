using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : RagdollEnemy
{
    WeaponIK weaponIK;
    public Transform currentTarget;
    EnemyManager enemyManager;

    public Gun gun;
    public float shootingTime = 1f;
    public State attackState;

    float nextActionTime = 0.0f;
    
    void Start()
    {
        weaponIK = GetComponentInChildren<WeaponIK>();
        currentTarget = weaponIK.targetTransform;
        enemyManager = GetComponent<EnemyManager>();
    }

    public void SetTarget(Transform target) {
        weaponIK.SetTargetTransform(target);
        currentTarget = target;
    }

    public override void Die() {
        foreach (Collider col in ragdollColliders) {
            col.enabled = true;
        }
        foreach (Rigidbody rig in ragdollRigidbodies) {
            rig.isKinematic = false;
        }
        body.isKinematic = true;
        anim.enabled = false;
        hitbox.enabled = false;
        body.constraints = RigidbodyConstraints.None;
        navMeshAgent.enabled = false;
        if (combatCollider != null) {
            combatCollider.SetActive(false);
        }
        foreach(HumanBone bone in weaponIK.humanBones){
            bone.weight = 0;
        }

    }
}
