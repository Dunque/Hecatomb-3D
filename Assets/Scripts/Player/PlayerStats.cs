using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    public HealthBar hbar;
    public PlayerController controller;
    public HeadBob shake;
    public ScreenFlash sf;
    public AudioClip[] hurtClips;

    public CapsuleCollider characterCollider;
    public CapsuleCollider characterCollisionBlockerCollider;

    public Collider enemyWeaponCollision;
    public float damageCooldown = 1;
    float timerCooldown = 0;
    bool weaponDamage = false;

    public override void Awake()
    {
        base.Awake();
        controller = GetComponent<PlayerController>();
        hbar = GetComponentInChildren<HealthBar>();
        sf = GetComponentInChildren<ScreenFlash>();
        hbar.SetMaxHealth(maxHp);
        enemyWeaponCollision = null;
    }

    private void Start() {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
    }

    public void DamageSound()
    {
        controller.playerAudioSource.PlayOneShot(hurtClips[Random.Range(0, hurtClips.Length - 1)]);
    }

    public void DamageCameraShake()
    {
        int n = Random.Range(0, 2);
        switch (n)
        {
            case (0):
                controller.shake.ShakeCamera(controller.shake.DoShakeDashR);
                break;
            case (1):
                controller.shake.ShakeCamera(controller.shake.DoShakeDashL);
                break;
        }
    }

    public override void ReceiveDamage(float damage)
    {
        currentHp -= damage;
        //Update healthbar
        hbar.SetHealth(currentHp);

        //Special effects
        DamageCameraShake();
        DamageSound();
        sf.FlashScreen();

        if (currentHp <= 0f)
        {
            currentHp = 0f;
            if (!isDead)
                Die();
            isDead = true;
        }
    }

    public override void ReceiveHealing(float heal)
    {
        base.ReceiveHealing(heal);

        //Update healthbar
        hbar.SetHealth(currentHp);
    }

    //Physical colliders, like melee weapons or explosions
    public override void OnTriggerEnter(Collider collider)
    {
        //only damaged by enemy hitboxes, in order to not collide with self hitboxes
        if (collider.tag == "EnemyHitbox" && weaponDamage == false)
        {
            timerCooldown = 0;
            weaponDamage = true;
            //Getting the data from the damaging hitbox
            HitboxStats hbs = collider.gameObject.GetComponent<HitboxStats>();
            ReceiveDamage(hbs.damage);

            if (canReceiveKnockback)
            {
                if (hbs.knockbackDir != Vector3.zero)
                    ReceiveKnockback(hbs.knockback, hbs.knockbackDir);
                else
                {
                    ReceiveKnockback(hbs.knockback, transform.position - collider.transform.position);
                }
            }
        }
    }

    public void Update() {
        if (weaponDamage) {
            timerCooldown += Time.deltaTime;
            if (timerCooldown >= damageCooldown) {
                weaponDamage = false;
                timerCooldown = 0;
            }
        }
    }
}
