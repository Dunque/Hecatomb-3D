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
    public AudioClip deathClip;

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
        if (damageable)
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
        StartCoroutine(IframesTimer(iframesTime));
    }

    public override void ReceiveHealing(float heal)
    {
        base.ReceiveHealing(heal);

        //Update healthbar
        hbar.SetHealth(currentHp);
    }

    public override void Die()
    {
        //Remove player movement input
        controller.playerInput = Vector2.zero;
        //Disable head bobbing
        controller.m_Camera.GetComponent<HeadBob>().enabled = false;
        //Disable weapon bobbing
        controller.wpnBob.enabled = false;
        //Disable arms sway with mouse movement
        controller.wpnSway.enabled = false;
        //play death animation and sound
        controller.anim.Play("Death");
        controller.playerAudioSource.PlayOneShot(deathClip);
        //Flash screen with blood
        sf.FlashAndStay();
        //Move and rotate the camera down, to resemble that the player is lying on the ground
        controller.m_Camera.transform.Translate(Vector3.down * 1.2f, Space.World);
        StartCoroutine(LookAtGroundLevel());
    }

    //This coroutine is in charge of rotating the camera's x angle towards 0, in order to look like the character is
    //lying flat in the ground.
    IEnumerator LookAtGroundLevel()
    {
        while(controller.m_Camera.transform.localEulerAngles.x > 0f)
        {
            Quaternion finalRot = Quaternion.Euler(Mathf.LerpAngle(controller.m_Camera.transform.localEulerAngles.x, 0f, Time.deltaTime * 4), 
                                                    controller.m_Camera.transform.localEulerAngles.y, 
                                                    controller.m_Camera.transform.localEulerAngles.z); 
            controller.m_Camera.transform.localRotation = finalRot;
            yield return null;
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

            if (canReceiveKnockback)
            {
                if (hbs.knockbackDir != Vector3.zero)
                    ReceiveKnockback(hbs.knockback, hbs.knockbackDir);
                else
                {
                    ReceiveKnockback(hbs.knockback, transform.position - collider.transform.position);
                }
            }

            ReceiveDamage(hbs.damage);
        }
    }
}
