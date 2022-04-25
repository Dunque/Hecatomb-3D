using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponDisplay : MonoBehaviour
{
    public TextMeshProUGUI ammoDisplay;
    public Image weaponImage;
    public Sprite noWeapon;

    public void Awake()
    {
        ammoDisplay = GetComponentInChildren<TextMeshProUGUI>();
        weaponImage = GetComponentInChildren<Image>();
    }

    public void EmptyText()
    {
        ammoDisplay.SetText("");
    }

    public void UpdateAmmoCount(int current, int max)
    {
        ammoDisplay.SetText("{0} | {1}", current, max);
    }

    public void EmptyImage()
    {
        weaponImage.sprite = noWeapon;
    }

    public void UpdateWeaponImage(Sprite icon)
    {
        weaponImage.sprite = icon;
    }
}
