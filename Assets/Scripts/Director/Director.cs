using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Director 
{
    public enum Scene
    {
        Menu,
        Level0,
        Level1,
        Level2
    }

    // Change to specified scene, load that scene
    public static void ChangeScene(Scene scene)
    {
        Cursor.visible = true;
        SceneManager.LoadScene(scene.ToString());
    }

    // Close the game
    public static void ExitProgram()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }


    // In a level, open pause menu
    public static void OpenPauseMenu()
    {
        Cursor.visible = true;
        // TODO
    }

    // In a level, open losing menu
    public static void OpenLosingMenu()
    {
        Cursor.visible = true;
        // TODO
    }
}
