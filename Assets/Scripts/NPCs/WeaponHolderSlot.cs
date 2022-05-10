using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public WeaponItem currentWeapon;
    public bool isLeftHandSlot;
    public bool isRightHandSlot;
    public bool isBackSlot;

    public GameObject currentWeaponModel;

    //Collider collider;

    public void UnloadWeapon() {
        if (currentWeaponModel != null) {
            currentWeaponModel.SetActive(false);
        }
    }
    
    public void UnloadWeaponAndDestroy() {
        if(currentWeaponModel != null) {
            Destroy(currentWeaponModel);
        }
    }

    public void LoadWeaponModel(WeaponItem weaponItem) {
        UnloadWeaponAndDestroy();

        if (weaponItem == null) {
            UnloadWeapon();
            return;
        }

        GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
        if(model != null) {
            //collider = model.GetComponent<BoxCollider>();
            //collider.enabled = false;
            if (parentOverride != null) {
                model.transform.position = gameObject.transform.position;
                model.transform.parent = parentOverride;
                
                
            } else {
                model.transform.parent = transform;
            }

            //model.transform.position = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }

        currentWeaponModel = model;
    }
}
