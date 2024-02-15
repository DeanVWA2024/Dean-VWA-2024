using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenController : MonoBehaviour
{
    public GameObject deathScreen;

    void Start()
    {
        // Ensure the death screen is initially disabled
        deathScreen.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        // Show the death screen
        deathScreen.SetActive(true);

        // Optionally, pause the game or perform other actions when the player dies
        Time.timeScale = 0f;
    }

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
}

