using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerState
{
    PlayerController character;

    public AirborneState(PlayerController character)
    {
        this.character = character;
    }

    public override void OnEnterState(PlayerController character)
    {
        Debug.Log("Entered Airborne State");
    }

    public override void HandleInput(PlayerController character)
    {
        character.SwingAirborne();

        if (Input.GetButtonUp("Jump") && !character.OnGround)     // Player stops pressing the jump button
            character.jumpCancel = true;

    }

    public override void Update(PlayerController character)
    {
        HandleInput(character);
    }

    public override void FixedUpdate(PlayerController character)
    {
    }

}
