using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public PlayerController player;
    public WeaponManager wpnManager;
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

        //_Quicki select weapon by pressin the numbers 1-3
        if (Input.GetKeyDown(KeyCode.Alpha1))
            weaponId = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            weaponId = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            weaponId = 3;

        wpnManager.ChangeGun(weaponId);
    }
}
