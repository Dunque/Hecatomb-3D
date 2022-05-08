using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
    [Header("References")]
    public Rigidbody body;
    public Collider hitbox;
    public EntityStats entityStats;

    AnimatorHandler animatorHandler;

    // Start is called before the first frame update
    public virtual void Awake() {
        hitbox = GetComponent<CapsuleCollider>();
        body = GetComponent<Rigidbody>();
        entityStats = GetComponent<EntityStats>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
    }
    
    // Update is called once per frame
    public virtual void Update() {

        if (entityStats.isDead) {
            hitbox.isTrigger = true;
            body.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

}
