using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PlayerState
{
    public virtual void OnEnterState(PlayerController character) { }
    public virtual void OnExitState(PlayerController character) { }
    public virtual void ToState(PlayerController character, PlayerState targetState)
    {
        character.State.OnExitState(character);
        character.State = targetState;
        character.State.OnEnterState(character);
    }
    public abstract void Update(PlayerController character);
    public abstract void FixedUpdate(PlayerController character);
    public virtual void HandleInput(PlayerController character) { }

}