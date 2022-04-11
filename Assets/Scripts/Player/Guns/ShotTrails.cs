using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTrails : MonoBehaviour
{

    [Header("Bullet tray")]
    [SerializeField] GameObject bulletTrail;
    [SerializeField] Transform muzzle;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] float fadeDuration = 5f;

    public void MuzzleExplosion()
    {
        muzzleFlash.Play();
    }
    
    //This function instantiates a bullet trail coming from the muzzle of the 
    //weapon, and it calls the fading coroutine for that trail.
    public void CreateTrail(Vector3 end)
    {
        LineRenderer lr = Instantiate(bulletTrail).GetComponent<LineRenderer>();
        lr.SetPositions(new Vector3[2] { muzzle.position, end });
        StartCoroutine(FadeTrail(lr));
    }

    //This function reduces the opacity of the line renderer, until it's invisible.
    //Then, it destroys it. For that purpose, we need to pass the object reference,
    //not only the line renderer.
    IEnumerator FadeTrail(LineRenderer lr)
    {
        float alphaEnd = 1;
        float alphaStart = 0.3f;
        while (alphaEnd > 0)
        {
            alphaStart -= Time.deltaTime / fadeDuration;
            alphaEnd -= Time.deltaTime / fadeDuration;
            lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alphaStart);
            lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, alphaEnd);
            yield return null;
        }
        Destroy(lr.gameObject);
    }
}
