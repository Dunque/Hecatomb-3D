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

        character.SwingGroundCombos();
    }

    public override void Update(PlayerController character)
    {
        HandleInput(character);
    }

    public override void FixedUpdate(PlayerController character)
    {
        Jump();
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