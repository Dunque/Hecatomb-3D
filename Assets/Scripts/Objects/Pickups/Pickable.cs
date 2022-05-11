using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    public bool pickedUp = false;
    public int amount = 40;
    public AudioSource audio_pickup;
    public GameObject visuals;

    public void Awake()
    {
        audio_pickup = GetComponent<AudioSource>();
    }

    public void Start()
    {
        Destroy(gameObject, 30f);
    }

    public abstract void OnTriggerEnter(Collider collider);
}
