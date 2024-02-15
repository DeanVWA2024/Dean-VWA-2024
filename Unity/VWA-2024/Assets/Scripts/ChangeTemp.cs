using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTemp : MonoBehaviour
{
    public float temperatureChangeRate = .5f; 
    public float changeFrequency = 1f;
    public GameObject Player;

    private float timeSinceLastChange;

    private void Start()
    {
        timeSinceLastChange = 0f;
    }

    private void Update()
    {
        timeSinceLastChange += Time.deltaTime;

        // Überprüfe, ob die Änderungsfrequenz erreicht wurde
        if (timeSinceLastChange >= changeFrequency)
        {
            // Ändere die Temperatur entsprechend der Änderungsrate
            Player.GetComponent<Ressources>().ChangeTemp(temperatureChangeRate,0,0);

            // Setze die Zeit seit der letzten Änderung zurück
            timeSinceLastChange = 0f;
        }
    }
}

