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
        SceneManager.LoadScene(scene.ToString());
    }

    // Change to the next scene
    public static void NextScene()
    {
        int sceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % 4;

        if (sceneIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        SceneManager.LoadScene(sceneIndex);
    }


    // Reset the actal scene, reload that scene
    public static void ResetScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    // Close the game
    public static void ExitProgram()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
