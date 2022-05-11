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
            collider.GetComponentInChildren<Fade>().FadeToBlack();
            //TODO añadir Director.nextScene o lo que sea

        }
    }
}
