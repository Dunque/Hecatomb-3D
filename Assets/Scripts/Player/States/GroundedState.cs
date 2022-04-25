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
                character.anim.Play(character.groundAttackData.animName[character.currentCombo]);
                character.anim.SetBool("holding", true);
                character.weaponHitbox.knockback = character.groundAttackData.knockback[character.currentCombo];
                character.weaponHitbox.knockbackDir = character.groundAttackData.knockbackDir[character.currentCombo];
                character.weaponHitbox.damage = character.groundAttackData.damage[character.currentCombo];
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                character.anim.Play("Parry");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                character.anim.Play("Stinger");
                character.weaponHitbox.knockback = 8f;
                character.weaponHitbox.knockbackDir = character.playerForward;
                character.weaponHitbox.damage = 10f;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                character.anim.Play("HTime");
                character.weaponHitbox.knockback = 12f;
                character.weaponHitbox.knockbackDir = (0.9f * Vector3.up + 0.1f * character.playerForward).normalized;
                character.weaponHitbox.damage = 10f;
            }

            if (Input.GetButtonDown("Fire2"))
            {
                character.wpnManager.ShootGun();
            }
            if (Input.GetButtonUp("Fire2"))
            {
                character.wpnManager.StopShootGun();
            }

        }
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
        //We only play the jumping animation if we are in the idle state.
        if (character.anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            character.anim.Play("Jumping");
        
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
        SFX_Dash();
    }
}