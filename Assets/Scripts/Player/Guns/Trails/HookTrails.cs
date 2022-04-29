using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.Guns
{
    public class HookTrails : ShotTrails
    {
        UpdateHookTrail uht;

        public void CreateTrail(Transform end)
        {
            //If there is already one hookTrail, we destroy it
            DestroyTrail();

            //We instantiate a new one and set it's position to the muzzle of the hook and the transform
            //of the grabbed object
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