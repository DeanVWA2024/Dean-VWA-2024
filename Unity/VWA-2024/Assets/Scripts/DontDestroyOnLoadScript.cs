using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    private void Awake()
    {
        // Check if there's another instance of this object in the scene
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        if (objs.Length > 1)
        {
            // Destroy the duplicate object
            Destroy(gameObject);
        }
        else
        {
            // Make this object persistent across scenes
            DontDestroyOnLoad(gameObject);
        }
    }
}
