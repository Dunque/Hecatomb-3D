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
        SwingAirborne();
    }

    public override void Update(PlayerController character)
    {
    }

    public override void FixedUpdate(PlayerController character)
    {
    }

    private void SwingAirborne()
    {
        if (character.canAirAttack)
        {
            if (Input.GetButtonDown("Attack1"))
            {
                character.anim.Play("AirSwing");
                character.anim.SetBool("holding", true);
                character.weaponHitbox.knockback = 15f;
                character.weaponHitbox.knockbackDir = Vector3.down;
                character.weaponHitbox.damage = 10f;
            }

        }
        if (Input.GetButtonDown("Attack2"))
        {
            character.wpnManager.ShootGun();
        }
        if (Input.GetButtonUp("Attack2"))
        {
            character.wpnManager.StopShootGun();
        }
    }
}
