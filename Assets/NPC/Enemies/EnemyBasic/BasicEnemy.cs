using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
    [Header("References")]
    public Rigidbody body;
    public Collider hitbox;
    public EntityStats entityStats;

    float hp;

    public float colorTime = 0.1f;
    public float colorTimer = 0f;


    private void ColorCooldown() {
        if (colorTimer > 0) {
            colorTimer -= Time.deltaTime;
        } else {
            colorTimer = 0;
        }
    }


    // Start is called before the first frame update
    void Awake() {
        hitbox = GetComponent<CapsuleCollider>();
        body = GetComponent<Rigidbody>();
        entityStats = GetComponent<EntityStats>();

        hp = entityStats.maxHp;
    }

    // Update is called once per frame
    void Update() {
        Ray ray = new Ray(transform.position, body.velocity);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        if (!entityStats.isDead) {
            if (entityStats.currentHp < hp) {
                colorTimer = colorTime;
            } else {
                ColorCooldown();
            }
        } else {
            
            hitbox.isTrigger = true;
            body.constraints = RigidbodyConstraints.FreezePosition;
        }
        hp = entityStats.currentHp;
    }

}
