using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerState
{
    PlayerController character;

    public GroundedState(PlayerController character)
    {
        this.character = character;
    }

    public override void OnEnterState(PlayerController character)
    {
        Debug.Log("Entered Grounded State");
        character.canAirAttack = true;
    }

    public override void HandleInput(PlayerController character)
    {

        SwingGroundCombos();

        if (Input.GetButtonDown("Fire3") && ((character.playerInput.x != 0f) || (character.playerInput.y != 0f)) && character.canDodge)
        {
            character.dashed = false;
        }
        if (Input.GetButtonDown("Jump"))
        {
            character.jump = true;
        }
    }

    public override void Update(PlayerController character)
    {
        HandleInput(character);
    }

    public override void FixedUpdate(PlayerController character)
    {
        if (character.jump)
            Jump();

        if (!character.dashed)
            Dash();
    }

    private void SwingGroundCombos()
    {
        if (character.canAttack)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                character.anim.Play("SwingV");
                character.anim.SetBool("holdV", true);
                character.weaponHitbox.knockback = 5f;
                character.weaponHitbox.knockbackDir = Vector3.zero;
                character.weaponHitbox.damage = 10f;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                character.anim.Play("Parry");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                character.anim.Play("Stinger");
                character.weaponHitbox.knockback = 8f;
                character.weaponHitbox.knockbackDir = Vector3.zero;
                character.weaponHitbox.damage = 10f;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                character.anim.Play("HTime");
                character.weaponHitbox.knockback = 15f;
                character.weaponHitbox.knockbackDir = Vector3.up;
                character.weaponHitbox.damage = 10f;
            }
        }

        if (character.canCombo)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                character.canCombo = false;
                character.anim.applyRootMotion = false;
                character.anim.CrossFade("2SwingV", 0.2f);
                character.anim.SetBool("2holdV", true);
                character.weaponHitbox.knockback = 6.5f;
                character.weaponHitbox.knockbackDir = Vector3.zero;
                character.weaponHitbox.damage = 12f;
            }
        }

        if (character.canCombo2)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                character.canCombo2 = false;
                character.anim.applyRootMotion = false;
                character.anim.CrossFade("SwingH", 0.2f);
                character.anim.SetBool("holdH", true);
                character.weaponHitbox.knockback = 9f;
                character.weaponHitbox.knockbackDir = Vector3.zero;
                character.weaponHitbox.damage = 20f;
            }
        }

        if (Input.GetButtonDown("Fire2"))
            character.anim.Play("Shoot");
    }
    private void SFX_Dash()
    {
        int n;

        n = Random.Range(0, 2);
        switch (n)
        {
            case (0):
                character.audio_Dash1.Play();
                break;
            case (1):
                character.audio_Dash2.Play();
                break;
        }
    }

    private void Dash()
    {
        var wishdir = new Vector3(character.playerInput.x, 0, character.playerInput.y);
        wishdir = character.trans.TransformDirection(wishdir);
        wishdir.Normalize();

        if (character.playerInput.x < 0f)
            character.shake.ShakeCamera(character.shake.DoShakeDashL);
        else if (character.playerInput.x > 0f)
            character.shake.ShakeCamera(character.shake.DoShakeDashR);

        character.velocity += wishdir * character.dodgeAmount;
        character.canDodge = false;
        character.isDashing = true;
        character.dodgeTimer = character.dodgeCooldown;
        character.isDashingTimer = character.dodgeCooldown - 0.4f;
        character.dashed = true;
        SFX_Dash();
    }

    private void Jump()
    {
        character.jump = false;

        Vector3 jumpDirection = Vector3.up;

         character.stepsSinceLastJump = 0;
        float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * character.jumpHeight);
        jumpDirection = (jumpDirection + Vector3.up).normalized;
        float alignedSpeed = Vector3.Dot(character.velocity, jumpDirection);
        if (alignedSpeed > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }
        character.velocity += jumpDirection * jumpSpeed;
    }
}