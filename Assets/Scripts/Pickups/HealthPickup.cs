using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public class HealthPickup : Pickable
    {

        public int amount = 40;

        public override void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Player")
            {
                PlayerStats controller = collider.GetComponentInParent<PlayerStats>();

                controller.ReceiveHealing(amount);
            }
        }
    }
}