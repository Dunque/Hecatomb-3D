using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TP_Rocas : MonoBehaviour
{
   public Transform[] target;



    void OnTriggerEnter (Collider other) { 
        if (other.CompareTag("Roca"))
        {
            int aux = target.Length;
            int i = Random.Range(0, aux);
        Debug.Log("SIIIIUUUU");
         Vector3 newPos = target[i].position;
         newPos.y = target[i].position.y;
         newPos.x = target[i].position.x;
         newPos.z = target[i].position.z;
        other.transform.position = newPos;
}}
}
