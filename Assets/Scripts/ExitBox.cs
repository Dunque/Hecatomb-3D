using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ExitBox : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // Turn screen to black
            collider.GetComponentInParent<PlayerStats>().fade.FadeToBlack();
            // Go to next scene
            Director.NextScene();

        }
    }
}
