using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public class AmmoBulletsPickup : Pickable
    {
        public override void OnTriggerEnter(Collider collider)
        {
            if (!pickedUp)
            {
                if (collider.tag == "Player")
                {
                    WeaponManager wpn = collider.GetComponentInParent<WeaponManager>();

                    //Checking if the player has said weapon
                    Gun gun = wpn.wpnRevolver.GetComponent<Gun>();

                    if (gun != null)
                    {
                        //If the player has full ammunition, it's not picked up
                        if (gun.currentAmmo != gun.maxAmmo)
                        {
                            //When picked up, it adds the ammo and then gets destroyed
                            gun.AddAmmo(amount);
                            audio_pickup.Play();
                            visuals.SetActive(false);
                            pickedUp = true;
                            Destroy(gameObject, 0.5f);
                        }
                    }
                }
            }
        }
    }
}