using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoBehaviour
{
    [SerializeField] float fadeDuration = 0.3f;
    Image sf;

    private void Awake()
    {
        sf = GetComponent<Image>();
    }

    public void FlashScreen()
    {
        sf.color = new Color(1, 1, 1, 1);
        StartCoroutine(FadeImage());
    }

    //This function reduces the opacity of the line renderer, until it's invisible.
    //Then, it destroys it. For that purpose, we need to pass the object reference,
    //not only the line renderer.
    IEnumerator FadeImage()
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            sf.color = new Color(sf.color.r, sf.color.g, sf.color.b, alpha);
            yield return null;
        }
    }
}
