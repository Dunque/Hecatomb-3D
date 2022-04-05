using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    [Header("References")]
    public new Renderer renderer;
    public Rigidbody body;
    public Collider hitbox;
    public EntityStats entityStats;

    float hp;

    public float colorTime = 0.1f;
    public float colorTimer = 0f;


    private void ColorCooldown()
    {
        if (colorTimer > 0)
        {
            colorTimer -= Time.deltaTime;
        }
        else
        {
            renderer.material.SetColor("_BaseColor", Color.white);
            colorTimer = 0;
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        hitbox = GetComponent<CapsuleCollider>();
        body = GetComponent<Rigidbody>();
        entityStats = GetComponent<EntityStats>();

        hp = entityStats.maxHp;
    }

    //Physical colliders, like melee weapons or explosions
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "hitbox")
        {
            //Getting the data from the damaging hitbox
            HitboxStats hbs = collider.gameObject.GetComponent<HitboxStats>();

            entityStats.ReceiveDamage(hbs.damage);

            if (hbs.knockbackDir != Vector3.zero)
                entityStats.ReceiveKnockback(hbs.knockback, hbs.knockbackDir);
            else
            {
                entityStats.ReceiveKnockback(hbs.knockback, transform.position - collider.transform.position);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, body.velocity);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        if (!entityStats.isDead) {
            if (entityStats.currentHp < hp)
            {
                renderer.material.SetColor("_BaseColor", Color.red);
                colorTimer = colorTime;
            }
            else
            {
                ColorCooldown();
            }
        }
        else
        {
            body.constraints = RigidbodyConstraints.None;
            renderer.material.SetColor("_BaseColor", Color.black);
        }
        hp = entityStats.currentHp;
    }

}
