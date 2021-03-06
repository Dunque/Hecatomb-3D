using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheelButtonController : MonoBehaviour
{
    public WeaponWheelController wpnWheel;
    public int ID;
    private Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    private bool selected = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        wpnWheel = GetComponentInParent<WeaponWheelController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            itemText.text = itemName;
        }
    }
    public void HoverEnter()
    {
        Debug.Log("hovered");
        anim.SetBool("Hover", true);
        itemText.text = itemName;
        selected = true;
        wpnWheel.wpnManager.ChangeGun(ID);
    }
    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        selected = false;
    }
}
