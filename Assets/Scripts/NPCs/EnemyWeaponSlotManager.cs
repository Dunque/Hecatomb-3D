using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : MonoBehaviour
{
    public WeaponItem rightHandWeapon; 
    public WeaponItem leftHandWeapon; 

    WeaponHolderSlot rightHandSlot;
    WeaponHolderSlot leftHandSlot;

    DamageCollider leftHandDamageCollider;
    DamageCollider rightHandDamageCollider;

    public PlayerStats playerStats;

    private void Awake() {
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots) {
            if (weaponSlot.isLeftHandSlot) {
                leftHandSlot = weaponSlot;
            }else if (weaponSlot.isRightHandSlot) {
                rightHandSlot = weaponSlot;
            }
        }
    }

    private void Start() {
        LoadWeaponsOnBothHands();
    }

    
    public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft) {
        if (isLeft) {
            leftHandSlot.currentWeapon = weapon;
            leftHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(true);
        } else {
            rightHandSlot.currentWeapon = weapon;
            rightHandSlot.LoadWeaponModel(weapon);
            LoadWeaponsDamageCollider(false);
        }
    }

    public void LoadWeaponsOnBothHands() {
        if (rightHandWeapon != null) {
            LoadWeaponOnSlot(rightHandWeapon, false);
        }
        if (leftHandWeapon != null) {
            LoadWeaponOnSlot(leftHandWeapon, true);
        }
    }

    public void LoadWeaponsDamageCollider(bool isLeft) {
        if (isLeft) {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        } else {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
    }

    public void OpenLeftDamageCollider() {
        if(leftHandDamageCollider != null)
            leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseLeftDamageCollider() {
        if (leftHandDamageCollider != null)
            leftHandDamageCollider.DisableDamageCollider();
    }

    public void OpenRightDamageCollider() {
        if (rightHandDamageCollider != null)
            rightHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightDamageCollider() {
        if (rightHandDamageCollider != null)
            rightHandDamageCollider.DisableDamageCollider();
    }

    public void CloseColliders() {
        CloseLeftDamageCollider();
        CloseRightDamageCollider();
    }
}
