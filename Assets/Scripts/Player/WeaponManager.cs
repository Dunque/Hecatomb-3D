using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public PlayerController controller;
    public WeaponDisplay wpnDisplay;

    public GameObject wpnNone;
    public GameObject wpnShotgun;
    public GameObject wpnRevolver;
    public GameObject wpnGrapplingHook;
    public GameObject[] gunList;
    public int currentGunIndex = 0;

    public Gun currentGun = null;

    public AudioClip changeWeaponClip;

    public void Awake()
    {
        controller = GetComponent<PlayerController>();
        wpnDisplay = GetComponentInChildren<WeaponDisplay>();

        //Adding weapons to the list
        //TODO temporal thing, remove this
        gunList = new GameObject[] { wpnNone, wpnShotgun, wpnRevolver, wpnGrapplingHook };
    }

    public void ChangeGun(int newGunIndex)
    {
        gunList[currentGunIndex].SetActive(false);
        currentGunIndex = newGunIndex;
        gunList[currentGunIndex].SetActive(true);
        currentGun = gunList[currentGunIndex].GetComponent<Gun>();

        //Update weapon display image
        if (currentGun != null)
            wpnDisplay.UpdateWeaponImage(currentGun.gunIcon);
        else
            wpnDisplay.EmptyImage();

        //Update ammo counter
        if (currentGun != null)
            wpnDisplay.UpdateAmmoCount(currentGun.currentAmmo, currentGun.maxAmmo);
        else
            wpnDisplay.EmptyText();

        //Play sound
        controller.playerAudioSource.PlayOneShot(changeWeaponClip);
    }

    public void ShootGun()
    {
        //currentGun == null means that the player has no equipped weapon.
        if (currentGun != null)
        {
            if (currentGun.CanShoot())
            {
                controller.anim.Play(currentGun.animName);
                wpnDisplay.UpdateAmmoCount(currentGun.currentAmmo, currentGun.maxAmmo);
            }
        }
    }

    public void StopShootGun()
    {
        //currentGun == null means that the player has no equipped weapon.
        if (currentGun != null)
            currentGun.StopShoot();
    }

}
