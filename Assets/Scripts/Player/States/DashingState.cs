using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : PlayerState
{
    PlayerController character;

    public DashingState(PlayerController character)
    {
        this.character = character;
    }

    public override void OnEnterState(PlayerController character)
    {
        Debug.Log("Entered Dashing State");
    }

    public override void HandleInput(PlayerController character)
    {
        character.SwingDashing();

        if (Input.GetButtonDown("Jump") && character.canJump) // Player starts pressing the button
        {
            character.jump = true;
            character.canJump = false;
        }
    }

    public override void Update(PlayerController character)
    {
        HandleInput(character);

        if (character.dashed && character.isDashingTimer <= 0f)
            ToState(character, character.groundedState);
    }

    public override void FixedUpdate(PlayerController character)
    {
        if (!character.dashed)
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
    }

    void SFX_Dash()
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

}
