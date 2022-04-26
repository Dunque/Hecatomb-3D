using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxStats : MonoBehaviour
{
    [SerializeField] public float knockback;
    [SerializeField] public Vector3 knockbackDir;
    [SerializeField] public float damage;

    public AudioSource audio_hit1;
    public AudioSource audio_hit2;
    public AudioSource audio_hit3;
    
    public void PlayHitSounds()
    {
        int n;

        n = Random.Range(0, 3);
        switch (n)
        {
            case (0):
                audio_hit1.Play();
                break;
            case (1):
                audio_hit2.Play();
                break;
            case (2):
                audio_hit3.Play();
                break;
        }
    }
}
