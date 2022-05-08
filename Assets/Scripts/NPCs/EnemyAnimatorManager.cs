using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : AnimatorManager
{
    EnemyManager enemyManager;
    EnemyWeaponSlotManager weaponSlotManager;

    Collider leftWeaponCollider;
    Collider rightWeaponCollider;

    private void Awake() {
        anim = GetComponent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();
        weaponSlotManager = GetComponent<EnemyWeaponSlotManager>();
    }

    private void OnAnimatorMove() {
        float delta = Time.deltaTime;
        enemyManager.enemyRigidBody.drag = 0;
        Vector3 deltaPosition = anim.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemyManager.enemyRigidBody.velocity = velocity;
    }

    public void EnableHitbox() {
        if (weaponSlotManager.leftHandWeapon != null) {
            leftWeaponCollider = weaponSlotManager.leftHandWeapon.modelPrefab.GetComponent<BoxCollider>();
            leftWeaponCollider.enabled = true;
        }
        if (weaponSlotManager.rightHandWeapon != null) {
            rightWeaponCollider = weaponSlotManager.rightHandWeapon.modelPrefab.GetComponent<BoxCollider>();
            rightWeaponCollider.enabled = true;
        }
    }
}
