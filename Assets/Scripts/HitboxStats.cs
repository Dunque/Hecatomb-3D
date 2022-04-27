using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxStats : MonoBehaviour
{
    [SerializeField] public float knockback;
    [SerializeField] public Vector3 knockbackDir;
    [SerializeField] public float damage;

    public AudioSource audioHit;
    public AudioClip[] hitSounds;
    
    public void PlayHitSounds()
    {
        audioHit.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length - 1)]);
    }
}
