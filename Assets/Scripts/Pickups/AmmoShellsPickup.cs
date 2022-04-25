using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public class AmmoShellsPickup : Pickable
    {
        public int amount = 6;
        public AudioSource audio_pickup;
        public GameObject visuals;

        public void Awake()
        {
            audio_pickup = GetComponent<AudioSource>();
        }

        public override void OnTriggerEnter(Collider collider)
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
                        audio_pickup.Play();
                        visuals.SetActive(false);
                        Destroy(gameObject, 0.5f);
                    }
                }
            }
        }
    }
}