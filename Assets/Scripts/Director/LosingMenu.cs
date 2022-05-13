using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosingMenu : MonoBehaviour
{
    public static bool aux = false;
    public static bool inLosingMenu = false;

    public GameObject losingMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (aux)
        {
            if (!inLosingMenu)
            {
                aux = false;
                Losing();
            }
        }
    }

    public void Losing()
    {
        losingMenuUI.SetActive(true);
        inLosingMenu = true;
    }

    public void Restart()
    {
        losingMenuUI.SetActive(false);
        inLosingMenu = false;
        Director.ResetScene();
    }

    public void ExitToMenu()
    {
        losingMenuUI.SetActive(false);
        inLosingMenu = false;
        Director.ChangeScene(Director.Scene.Menu);
    }
}
