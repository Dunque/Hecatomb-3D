using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelController : MonoBehaviour
{
    public PlayerController player;
    public WeaponManager wpnManager;
    public Animator anim;

    public void OpenWheel()
    {
        anim.SetBool("OpenWeaponWheel", true);
        player.mouseLook.SetCursorLock(false);
    }

    public void CloseWheel()
    {
        anim.SetBool("OpenWeaponWheel", false);
        player.mouseLook.SetCursorLock(true);
    }
}
