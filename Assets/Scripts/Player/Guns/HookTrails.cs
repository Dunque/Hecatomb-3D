using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.Guns
{
    public class HookTrails : ShotTrails
    {
        UpdateHookTrail uht;

        public override void CreateTrail(Vector3 end)
        {
            if (uht)
                Destroy(uht.gameObject);
            uht = Instantiate(bulletTrail).GetComponent<UpdateHookTrail>();
            uht.SetPositions(muzzle, end);
        }

        public void CreateTrail(Transform end)
        {
            if (uht)
                Destroy(uht.gameObject);
            uht = Instantiate(bulletTrail).GetComponent<UpdateHookTrail>();
            uht.SetPositions(muzzle, end);
        }

        public void DestroyTrail()
        {
            if (uht)
                Destroy(uht.gameObject);
        }

    }
}