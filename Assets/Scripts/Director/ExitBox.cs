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
            // Go to next scene after the screen fades
            StartCoroutine(DelayNext(1.7f));
        }
    }

    IEnumerator DelayNext(float delayTime)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);

        //Do the action after the delay time has finished.
        Director.NextScene();
    }

}
