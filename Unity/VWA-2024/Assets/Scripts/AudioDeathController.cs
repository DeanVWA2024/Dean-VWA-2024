using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDeathController : MonoBehaviour
{
    private int i = 1;
    void Update()
    {
        if(gameObject.GetComponent<EnemyController>().currentHealth <= 0)
        {
            if(i == 1)
            {
                Debug.Log("Playing sound");
                FindObjectOfType<AudioManager>().Play("die");
                i++;
            }
        }
    }
}
