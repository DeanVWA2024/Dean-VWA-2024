using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void RestartGame()
    {
        Debug.Log("restarting...");

        // Reload the current scene (assuming your game has only one scene)
        DontDestroyOnLoadScript[] scripts = GameObject.FindObjectsOfType<DontDestroyOnLoadScript>();
        foreach (DontDestroyOnLoadScript script in scripts)
        {
            Destroy(script.gameObject);
        }

        SceneManager.LoadScene("Cave");


        // Optionally, resume time scale if you had paused it
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
