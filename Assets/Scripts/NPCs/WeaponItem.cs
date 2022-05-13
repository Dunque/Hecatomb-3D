using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Idle Animations")]
    public string right_hand_idle;
    public string left_hand_idle;
    public string th_hand_idle;

    [Header("Attack Animations")]
    public string oh_light_attack_01;
    public string oh_light_attack_02;
    public string th_light_attack_01;
    public string th_light_attack_02;

    [Header("Stamina Costs")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;
    
}
