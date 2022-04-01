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

        if (Input.GetButtonDown("Fire3") && ((character.playerInput.x != 0f) || (character.playerInput.y != 0f)) && character.OnGround && character.canDodge)
        {
            character.dashed = false;
            ToState(character, character.dashingState);
        }
        if (Input.GetButtonDown("Jump") && character.canJump) // Player starts pressing the button
        {
            character.jump = true;
            character.canJump = false;
        }
    }

    public override void Update(PlayerController character)
    {
        HandleInput(character);
    }

    public override void FixedUpdate(PlayerController character)
    {
        Jump();
    }

    private void SwingGroundCombos()
    {
        if (character.canAttack)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                character.anim.Play("SwingV");
                character.anim.SetBool("holdV", true);
                character.knockback = 5f;
                character.damage = 10f;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                character.anim.Play("Parry");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                character.anim.Play("Stinger");
                character.knockback = 5f;
                character.damage = 10f;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                character.anim.Play("HTime");
                character.knockback = 5f;
                character.damage = 10f;
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
                character.knockback = 6.5f;
                character.damage = 12f;
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
                character.knockback = 9f;
                character.damage = 20f;
            }
        }
    }

    private void Jump()
    {
        if (character.jump)
        {
            Vector3 jumpDirection = Vector3.up;

            character.jump = false;

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
        // Cancel the jump when the button is no longer pressed
        if (character.jumpCancel)
        {
            if (character.velocity.y > character.jumpHeightShort)
                character.velocity.y = character.jumpHeightShort;
            character.jumpCancel = false;
        }
    }
}