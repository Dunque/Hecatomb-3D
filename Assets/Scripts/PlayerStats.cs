using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    public HealthBar hbar;
    public PlayerController controller;
    public HeadBob shake;
    public ScreenFlash sf;
    public AudioSource audio_hurt1;
    public AudioSource audio_hurt2;
    public AudioSource audio_hurt3;
    public AudioSource audio_hurt4;

    public override void Awake()
    {
        base.Awake();
        controller = GetComponent<PlayerController>();
        hbar = GetComponentInChildren<HealthBar>();
        sf = GetComponentInChildren<ScreenFlash>();
        hbar.SetMaxHealth(maxHp);
    }

    public void DamageSound()
    {
        int n = Random.Range(0, 3);
        switch (n)
        {
            case (0):
                audio_hurt1.Play();
                break;
            case (1):
                audio_hurt2.Play();
                break;
            case (2):
                audio_hurt3.Play();
                break;
            case (3):
                audio_hurt4.Play();
                break;
        }
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
        hbar.SetHealth(currentHp);

        DamageCameraShake();
        DamageSound();
        sf.FlashScreen();

        if (currentHp <= 0f)
        {
            isDead = true;
            currentHp = 0f;
        }
    }

    //Physical colliders, like melee weapons or explosions
    public override void OnTriggerEnter(Collider collider)
    {
        //only damaged by enemy hitboxes, in order to not collide with self hitboxes
        if (collider.tag == "EnemyHitbox")
        {
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
}
