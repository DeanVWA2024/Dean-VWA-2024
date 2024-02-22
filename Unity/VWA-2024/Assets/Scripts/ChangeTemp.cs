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

        // �berpr�fe, ob die �nderungsfrequenz erreicht wurde
        if (timeSinceLastChange >= changeFrequency)
        {
            // �ndere die Temperatur entsprechend der �nderungsrate
            Player.GetComponent<Ressources>().ChangeTemp(temperatureChangeRate,0,0);

            // Setze die Zeit seit der letzten �nderung zur�ck
            timeSinceLastChange = 0f;
        }
    }
}

