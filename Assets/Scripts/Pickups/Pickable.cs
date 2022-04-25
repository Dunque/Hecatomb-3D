using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickable : MonoBehaviour
{
    public abstract void OnTriggerEnter(Collider collider);
}
