using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Director.ChangeScene(Director.Scene.Level0);
    }

    public void ExitGame()
    {
        Director.ExitProgram();
    }
}
