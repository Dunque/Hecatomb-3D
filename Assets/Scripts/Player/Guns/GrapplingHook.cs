using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.Guns
{
    public class GrapplingHook : Gun
    {
        public HookTrails hookTrails;
        public PlayerController controller;
        public float grapplePower = 350f;
        public float minDistanceToPlayer = 5f;

        public Transform grappleTrans;
        public bool grappled = false;

        EntityStats entStats;
        public bool grappledEnemy = false;

        public override void Awake()
        {
            controller = GetComponentInParent<PlayerController>();
            range = 50f;
            damage = 5f;
            knockback = 0.7f;
            maxAmmo = 1;
            animName = "GrapplingHook";
            base.Awake();
        }

        public override void Shoot()
        {
            hookTrails.MuzzleExplosion();

            RaycastHit hit;
            Vector3 shootingDir = GetShootingDir();

            //Layer 10 represents the grabable object (enemies and certain platforms)
            if (Physics.SphereCast(cam.position, 0.1f, shootingDir, out hit, range, (1 << 10)))
            {
                grappleTrans = hit.transform;

                //If we grab a barrel, crate or enemy we pull them towards us
                if ((entStats = hit.collider.GetComponent<EntityStats>()) != null)
                {
                    hookTrails.CreateTrail(grappleTrans);
                    grappledEnemy = true;
                    entStats.ReceiveDamage(damage);
                }
                //If we grab a grabable part of the scenery, we pull ourselves towards it
                else
                {
                    hookTrails.CreateTrail(grappleTrans);
                    grappled = true;
                }
            }
        }

        public override void StopShoot()
        {
            grappled = false;
            grappledEnemy = false;
            hookTrails.DestroyTrail();
            entStats = null;
        }

        public void FixedUpdate()
        {
            //Grabbed terrain, we pull ourselves to it
            if (grappled)
            {
                Vector3 wishdir = grappleTrans.position - controller.transform.position;
                wishdir.Normalize();

                controller.body.AddForce(wishdir * grapplePower);
            }
            //Grabbed enemy, we pull it towards us
            if (grappledEnemy)
            {
                Vector3 wishdir = controller.transform.position - grappleTrans.position;
                wishdir.Normalize();

                //Check object proximity in order to not generate infinite speed towards the player
                if ((controller.transform.position - entStats.transform.position).magnitude > minDistanceToPlayer)
                    entStats.ReceiveKnockback(knockback, wishdir);
            }
        }


        public override Vector3 GetShootingDir()
        {
            Vector3 targetPos = cam.position + cam.forward * range;
            Vector3 dir = targetPos - cam.position;
            return dir.normalized;
        }
    }
}