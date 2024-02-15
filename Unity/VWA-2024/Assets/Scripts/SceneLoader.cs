using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string newSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Your collider collided with the player!");
            collision.transform.position = Vector3.zero;
            LoadNewScene(newSceneName);
        }
    }

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
