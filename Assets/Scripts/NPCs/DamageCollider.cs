using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    Collider damageCollider;

    public float currentWeaponDamage = 25;

    private void Awake() {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    public void EnableDamageCollider() {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider() {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider collision) {
        if (collision.tag == "Hittable") {
            EntityStats entStats = collision.GetComponent<PlayerStats>();
            if(entStats != null) {
                entStats.ReceiveDamage(currentWeaponDamage);
            }
        }
    }
}
