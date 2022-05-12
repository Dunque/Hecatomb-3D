using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    [SerializeField] float fadeDuration = 0.3f;
    public Image bloodyFlash;
    public Image deathFlash;

    public void FlashAndStay()
    {
        deathFlash.color = new Color(1, 1, 1, 1);
    }

    public void FlashScreen()
    {
        bloodyFlash.color = new Color(1, 1, 1, 1);
        StartCoroutine(FadeImage());
    }

    //This function reduces the opacity of the image, until it's invisible.
    IEnumerator FadeImage()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            bloodyFlash.color = new Color(bloodyFlash.color.r, bloodyFlash.color.g, bloodyFlash.color.b, alpha);
            yield return null;
        }
    }
}
