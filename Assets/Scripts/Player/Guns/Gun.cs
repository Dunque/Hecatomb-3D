using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{

    public Transform cam;

    [Header("Weapon Stats")]
    [SerializeField] public float range;
    [SerializeField] public float damage;
    [SerializeField] public float knockback;

    [SerializeField] public int maxAmmo;
    public int currentAmmo;

    public ShotTrails shotTrails;

    public Light muzzle;
    public AudioSource sound;

    [Header("Animation")]
    public Sprite gunIcon;
    [SerializeField] public string animName;

    public LayerMask ignoreLayers;

    public bool isPlayer;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        Camera camera = GetComponentInParent<Camera>();
        shotTrails = GetComponentInParent<ShotTrails>();
        if(camera!=null)
            cam = camera.transform;
        currentAmmo = maxAmmo;
        ignoreLayers = LayerMask.GetMask("CharacterCollisionBlocker");
    }

    public virtual void Shoot()
    {
        shotTrails.MuzzleExplosion();
        if(muzzle != null && !isPlayer) {
            muzzle.enabled = true;
            StartCoroutine(TurnDownMuzzle());
        }
        if (sound != null) {
            sound.Play();
        }
    }

    IEnumerator TurnDownMuzzle() {
        for (int i = 1; i < 5; i++) {
            muzzle.intensity = i * 2;
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 5; i > 1; i--) {
            muzzle.intensity = i * 2;
            yield return new WaitForSeconds(0.05f);
        }
        muzzle.enabled = false;
    }

    public virtual void StopShoot() {
    }

    public void AddAmmo(int amount)
    {
        if (currentAmmo + amount > maxAmmo)
            currentAmmo = maxAmmo;
        else
            currentAmmo = currentAmmo + amount;
    }

    public bool CanShoot()
    {
        return (currentAmmo > 0);
    }

    public abstract Vector3 GetShootingDir();
}
