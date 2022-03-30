using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    [Header("References")]
    public new Renderer renderer;
    public Rigidbody body;
    public Collider hitbox;

    public bool isHit;

    public bool isDead;

    public float colorTime = 0.1f;
    public float colorTimer = 0f;

    [Header("Stats")]
    [SerializeField] public float hp = 50f;


    private void ColorCooldown()
    {
        if (colorTimer > 0)
        {
            colorTimer -= Time.deltaTime;
        }
        else
        {
            isHit = false;
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
        isDead = false;

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "hitbox")
        {
            renderer.material.SetColor("_BaseColor", Color.red);
            isHit = true;

            Vector3 dir = transform.position - collider.gameObject.GetComponentInParent<PlayerController>().transform.position;
            //Vector3 dir = transform.position - collider.transform.position;
            hp -= collider.gameObject.GetComponentInParent<PlayerController>().damage;

            dir = dir.normalized;
            body.velocity += dir * collider.gameObject.GetComponentInParent<PlayerController>().knockback;
            //* collider.GetComponentInParent<PlayerController>().body.velocity.magnitude
        }

    }
    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("An object left.");
    //}

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, body.velocity);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.red);

        if (!isDead) {
            if (!isHit)
            {
                colorTimer = colorTime;
            }
            else
            {
                ColorCooldown();
            }
        }
            

        if (hp <= 0f) {
            isDead = true;
            body.constraints = RigidbodyConstraints.None;
            renderer.material.SetColor("_BaseColor", Color.black);
            //Destroy(gameObject);
        }
        
    }

}
