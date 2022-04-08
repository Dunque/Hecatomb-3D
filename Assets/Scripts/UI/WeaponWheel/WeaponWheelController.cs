using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public PlayerController player;
    public Animator anim;
    private bool weaponWheelSelected = false;
    public static int weaponId = 0;

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

        player.ChangeGun(weaponId);
    }
}
