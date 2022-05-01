using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public class AmmoShellsPickup : Pickable
    {
        public override void OnTriggerEnter(Collider collider)
        {
            if (!pickedUp)
            {
                if (collider.tag == "Player")
                {
                    WeaponManager wpn = collider.GetComponentInParent<WeaponManager>();

                    //Checking if the player has said weapon
                    Gun gun = wpn.wpnShotgun.GetComponent<Gun>();

                    if (gun != null)
                    {
                        //If the player has full ammunition, it's not picked up
                        if (gun.currentAmmo != gun.maxAmmo)
                        {
                            //When picked up, it adds the ammo and then gets destroyed
                            gun.AddAmmo(amount);
                            //Update ammo counter
                            wpn.UpdateAmmoCount();
                            //play sound effect
                            audio_pickup.Play();
                            //Deactivate visuals
                            visuals.SetActive(false);
                            pickedUp = true;
                            //Destroy it
                            Destroy(gameObject, 0.5f);
                        }
                    }
                }
            }
        }
    }
}