using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnemy : BasicEnemy
{
    public List<Collider> ragdollColliders = new List<Collider>();
    public List<Rigidbody> ragdollRigidbodies = new List<Rigidbody>();
    public Animator anim;

    public override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("hasRagdoll", true);
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rig in ragdollRigidbodies)
        {
            rig.isKinematic = true;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("FinishedDying"))
        {
            foreach (Collider col in ragdollColliders)
            {
                col.enabled = true;
            }
            foreach (Rigidbody rig in ragdollRigidbodies)
            {
                rig.isKinematic = false;
            }
            body.isKinematic = true;
            anim.enabled = false;
            hitbox.enabled = false;
            body.constraints = RigidbodyConstraints.None;
        }
    }
}
