using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // TODO: preload scene and then just switch when the button is clicked
    public void StartGame()
    {
        SceneManager.LoadScene("Newspaper");
    }

    // TODO
    public void GoToCredits()
    {

    }

    public void GoToStart()
    {

    }
}