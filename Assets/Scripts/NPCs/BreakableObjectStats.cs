using System.Collections;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public class BreakableObjectStats : EntityStats
    {
        public GameObject brokenVersion;
        public float fadeDuration = 2f;
        public float force = 20f;

        public GameObject[] drops;

        public override void Die()
        {
            //Instantiating the broken model, and getting a reference to each part that it has
            GameObject broken = Instantiate(brokenVersion, transform.position, transform.rotation);

            //Instantiating the ammo/health drops from the crate
            GameObject pickup = Instantiate(drops[Random.Range(0, drops.Length)], transform.position, transform.rotation);
            pickup.GetComponent<Rigidbody>().AddForce(Vector3.up * 3f);

            //Break up the pieces with a bit of outwards force
            body.AddExplosionForce(force, Vector3.zero, 2f);

            //Destroy the original object
            Destroy(gameObject);

            //Destroy the parts on a timer
            Destroy(broken, 5f);
        }
    }
}