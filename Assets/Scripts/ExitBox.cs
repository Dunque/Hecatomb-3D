using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBox : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            //turn screen to black
            collider.GetComponentInParent<PlayerStats>().fade.FadeToBlack();
            //TODO a�adir Director.nextScene o lo que sea

        }
    }
}
