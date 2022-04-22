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

        public Vector3 grapplePoint;
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

            if (Physics.SphereCast(cam.position, 0.1f, shootingDir, out hit, range))
            {

                if ((entStats = hit.collider.GetComponent<EntityStats>()) != null)
                {
                    entStats.ReceiveDamage(damage);
                    grappledEnemy = true;
                    hookTrails.CreateTrail(hit.transform);
                }
                else
                {
                    grapplePoint = hit.point;
                    grappled = true;
                    hookTrails.CreateTrail(hit.point);
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
            if (grappled)
            {
                Vector3 wishdir = grapplePoint - controller.transform.position;
                wishdir.Normalize();

                controller.body.AddForce(wishdir * grapplePower);
            }

            if (grappledEnemy)
            {
                Vector3 wishdir = controller.transform.position - entStats.transform.position;
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