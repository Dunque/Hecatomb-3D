using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public PlayerController player;
    public Animator anim;
    private bool weaponWheelSelected = false;
    public static int weaponId = -1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            weaponWheelSelected = true;
        }
        else
        {
            weaponWheelSelected = false;
        }

        if (weaponWheelSelected)
        {
            anim.SetBool("OpenWeaponWheel", true);
            player.mouseLook.SetCursorLock(false);
        }
        else
        {
            anim.SetBool("OpenWeaponWheel", false);
            player.mouseLook.SetCursorLock(true);
        }

        switch (weaponId)
        {
            case 0:
                player.ChangeGun(0);
                break;
            case 1:
                player.ChangeGun(1);
                break;
            default:
                break;
        }
    }
}
