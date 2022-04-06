using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponWheelButtonController : MonoBehaviour
{

    public int ID;
    private Animator anim;
    public string itemName;
    public TextMeshProUGUI itemText;
    private bool selected = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        WeaponWheelController.weaponId = ID;
    }
    public void HoverExit()
    {
        anim.SetBool("Hover", false);
        selected = false;
    }
}
