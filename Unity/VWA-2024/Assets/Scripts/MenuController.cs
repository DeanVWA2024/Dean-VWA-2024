using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public bool menuActive;

    void Start()
    {
        // Ensure the menu is initially disabled
        menuPanel.SetActive(false);
    }

    void Update()
    {
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the menu on/off
            ToggleMenu();
        }

        if(menuPanel.activeSelf == true)
        {
            menuActive = true;
        }
        else
        {
            menuActive = false;
        }
    }

    public void ToggleMenu()
    {
        // Toggle the menu panel's visibility
        menuPanel.SetActive(!menuPanel.activeSelf);

        // Pause the game when the menu is active
        Time.timeScale = (menuPanel.activeSelf) ? 0f : 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}

