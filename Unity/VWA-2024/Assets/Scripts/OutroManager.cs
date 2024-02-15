using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroManager : MonoBehaviour
{
    public GameObject Outro;

    private void Awake()
    {
        Outro = GameObject.FindGameObjectWithTag("Outro");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (Transform child in Outro.transform)
            {
                // Assuming you want to disable the entire GameObject of each child
                child.gameObject.SetActive(true);
            }

            Time.timeScale = 0f;
        }
    }
}
