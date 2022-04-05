using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float maxHp = 50f;
    [SerializeField] public float currentHp;
    public Rigidbody body;
    public bool isDead;

    void Awake()
    {
        currentHp = maxHp;
        body = GetComponent<Rigidbody>();
    }

    public void ReceiveDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0f)
        {
            isDead = true;
        }
    }

    public void ReceiveKnockback(float magnitude, Vector3 dir)
    {
        body.velocity += magnitude * dir;
    }
}
