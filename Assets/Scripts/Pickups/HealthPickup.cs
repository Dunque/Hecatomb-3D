using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public class HealthPickup : Pickable
    {
        public int amount = 40;
        public AudioSource audio_pickup;
        public GameObject visuals;

        public override void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Player")
            {
                PlayerStats stats = collider.GetComponentInParent<PlayerStats>();

                //If the player has full hp, it's not picked up
                if (stats.currentHp != stats.maxHp)
                {
                    //When picked up, it adds the hp and then gets destroyed
                    stats.ReceiveHealing(amount);
                    audio_pickup.Play();
                    visuals.SetActive(false);
                    Destroy(gameObject, 0.5f);
                }
            }
        }
    }
}