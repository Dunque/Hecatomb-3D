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

    // ------------------------------ Rootmotion
    public void Event_RestoreRootAnim()
    {
        anim.applyRootMotion = true;
        wpnswy.enabled = true;
    }
    public void Event_DisableRootAnim()
    {
        anim.applyRootMotion = false;
        wpnswy.enabled = false;
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

    // ------------------------------ Disable / Enable combos
    public void Event_DisableCombo()
    {
        controller.canCombo = false;
    }

    public void Event_EnableCombo()
    {
        controller.canCombo = true;
    }
    public void Event_DisableCombo2()
    {
        controller.canCombo2 = false;
    }

    public void Event_EnableCombo2()
    {
        controller.canCombo2 = true;
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
        int n;

        n = Random.Range(0, 3);
        switch (n)
        {
            case (0):
                controller.audio_SV.Play();
                break;
            case (1):
                controller.audio_SV2.Play();
                break;
            case (2):
                controller.audio_SV3.Play();
                break;
        }
    }

    public void Event_SFX_SwingH()
    {
        controller.audio_SH.Play();
    }
    public void Event_SFX_Stinger()
    {
        int n;

        n = Random.Range(0, 2);
        switch (n)
        {
            case (0):
                controller.audio_Stinger1.Play();
                break;
            case (1):
                controller.audio_Stinger2.Play();
                break;
        }
    }

    // ----------------------------------- AirDash
    public void AirDash()
    {
        var wishdir = controller.camTrans.forward;
        wishdir.Normalize();

        //controller.body.velocity += wishdir * controller.airDashAmount;
        controller.body.AddForce(wishdir * controller.airDashAmount);
    }

    // ------------------------------ Awake
    void Awake()
    {
        wpnswy = GetComponentInChildren<WeaponSway>();
        hitbox = GetComponentInChildren<BoxCollider>();
        anim = GetComponentInChildren<Animator>();
        controller = GetComponentInParent<PlayerController>();
        defaultSpeed = controller.maxSpeed;
        halfSpeed = defaultSpeed * 0.5f;
        shake = GetComponentInParent<HeadBob>();
        swordTrail.Stop();
    }

}
