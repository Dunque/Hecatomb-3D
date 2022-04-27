using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    public Animator anim;
    public PlayerController controller;
    public WeaponSway wpnswy;
    public HeadBob shake;
    public float defaultSpeed;
    public float halfSpeed;
    public Collider hitbox;
    public Collider hitboxStinger;
    public ParticleSystem swordTrail;

    void Awake()
    {
        wpnswy = GetComponentInChildren<WeaponSway>();
        hitbox = GetComponentInChildren<BoxCollider>();
        anim = GetComponentInChildren<Animator>();
        controller = GetComponentInParent<PlayerController>();
        defaultSpeed = controller.maxSpeed;
        halfSpeed = defaultSpeed * 0.9f;
        shake = GetComponentInParent<HeadBob>();
        swordTrail.Stop();
    }
    // ------------------------------ Shooting
    public void Event_Shoot()
    {
        controller.wpnManager.currentGun.Shoot();
    }

    // ------------------------------ Disable / Enable attacks
    public void Event_DisableAttack()
    {
        controller.canAttack = false;
    }

    public void Event_EnableAttack()
    {
        controller.canAttack = true;
    }

    public void Event_DisableAirAttack()
    {
        controller.canAirAttack = false;
    }

    public void Event_EnableAirAttack()
    {
        controller.canAirAttack = true;
    }

    // ------------------------------ Combo attack
    public void Event_NextCombo()
    {
        controller.canAttack = true;

        if (controller.currentCombo == 2)
            controller.currentCombo = 0;
        else
            controller.currentCombo++;
    }

    public void Event_ResetCombo()
    {
        controller.canAttack = true;

        controller.currentCombo = 0;
    }

    // ------------------------------ Speed related
    public void Event_ReduceSpeed()
    {
        controller.maxSpeed = halfSpeed;
    }

    public void Event_RestoreSpeed()
    {
        controller.maxSpeed = defaultSpeed;
    }

    // ------------------------------ Camera shake
    public void Event_ShakeV()
    {
        shake.ShakeCamera(shake.DoShakeV);
    }

    public void Event_ShakeV2()
    {
        shake.ShakeCamera(shake.DoShakeV2);
    }

    public void Event_ShakeH()
    {
        shake.ShakeCamera(shake.DoShakeH);
    }

    public void Event_ShakeH2()
    {
        shake.ShakeCamera(shake.DoShakeH2);
    }
    public void Event_ShakeAir()
    {
        shake.ShakeCamera(shake.DoShakeAir);
    }

    // ------------------------------ Disable / Enable hitboxes
    public void Event_EnableHitbox()
    {
        hitbox.enabled = true;
        swordTrail.Play();
    }
    public void Event_DisableHitbox()
    {
        hitbox.enabled = false;
        swordTrail.Stop();
    }

    public void Event_EnableHitboxStinger()
    {
        hitboxStinger.enabled = true;
        swordTrail.Play();
    }
    public void Event_DisableHitboxStinger()
    {
        hitboxStinger.enabled = false;
        swordTrail.Stop();
    }

    // --------------------------------- SFX
    public void Event_SFX_Swing()
    {
        controller.playerAudioSource.PlayOneShot(controller.lightSwingClips[Random.Range(0, controller.lightSwingClips.Length - 1)]);
    }

    public void Event_SFX_SwingH()
    {
        controller.playerAudioSource.PlayOneShot(controller.heavySwingClips[Random.Range(0, controller.heavySwingClips.Length - 1)]);
    }

    public void Event_SFX_Stinger()
    {
        controller.playerAudioSource.PlayOneShot(controller.thrustClips[Random.Range(0, controller.thrustClips.Length - 1)]);
    }

    public void Event_SFX_Shotgun()
    {
        controller.playerAudioSource.PlayOneShot(controller.shotgunClips[Random.Range(0, controller.shotgunClips.Length - 1)]);
    }

    public void Event_SFX_Revolver()
    {
        controller.playerAudioSource.PlayOneShot(controller.revolverClips[Random.Range(0, controller.revolverClips.Length - 1)]);
    }

    public void Event_SFX_GrapplingHook()
    {
        controller.playerAudioSource.PlayOneShot(controller.grapplingHookClips[Random.Range(0, controller.grapplingHookClips.Length - 1)]);
    }

    // ----------------------------------- AirDash
    public void AirDash()
    {
        var wishdir = controller.camTrans.forward;
        wishdir.Normalize();

        //controller.body.velocity += wishdir * controller.airDashAmount;
        controller.body.AddForce(wishdir * controller.airDashAmount);
    }

}
