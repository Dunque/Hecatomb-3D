using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public PlayerController controller;
    public WeaponDisplay wpnDisplay;
    [Space]
    public GameObject wpnNone;
    public GameObject wpnShotgun;
    public GameObject wpnRevolver;
    public GameObject wpnGrapplingHook;
    public GameObject[] gunList;
    public int currentGunIndex = 0;
    [Space]
    public Gun currentGun = null;
    [Header("Sounds")]
    public AudioClip changeWeaponClip;
    public AudioClip[] shotgunClips;
    public AudioClip[] revolverClips;
    public AudioClip[] grapplingHookClips;

    public void Awake()
    {
        controller = GetComponent<PlayerController>();
        wpnDisplay = GetComponentInChildren<WeaponDisplay>();

        //Initialising the weapon list
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
            UpdateAmmoCount();
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
            //If the weapon has enough ammo
            if (currentGun.CanShoot())
            {
                controller.anim.Play(currentGun.animName);
            }
        }
    }

    public void StopShootGun()
    {
        //currentGun == null means that the player has no equipped weapon.
        if (currentGun != null)
            currentGun.StopShoot();

    }

    public void UpdateAmmoCount()
    {
        wpnDisplay.UpdateAmmoCount(currentGun.currentAmmo, currentGun.maxAmmo);
    }

}
