using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RagdollEnemy : BasicEnemy
{
    public List<Collider> ragdollColliders = new List<Collider>();
    public List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
    public Animator anim;

    NavMeshAgent navMeshAgent;
    public GameObject combatCollider;

    public override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rig in ragdollRigidbodies)
        {
            rig.isKinematic = true;
        }
        navMeshAgent = GetComponentInChildren<NavMeshAgent>();
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
        
    }
}
