using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using JetBrains.Annotations;

public class Ressources : MonoBehaviour
{
    // Weapons
    public int remainingBullets;
    public int maxBullets;

    // Energy
    public float energyLeft;
    public float maxEnergy;

    // Temperature
    public float currentTemperature;
    public float minTemperature;
    public float maxTemperature;
    public float temperatureDeviation;
    public Vector2 activationTemperature;
    public bool isCold;
    public bool isHot;
    public bool regulationOn;
    public float regulationStrength;
    public float regulationEnergyUse;
    public float optimalTemp;
    public GameObject TemperaturePanel;
    public float temperaturePanelDeviation;
    public float panelDeltaDeviation;
    public float damageTicker = 1;

    // Clock
    public float tickEnergyTimeDelta;
    public float timer = 0;

    // Energy Drain
    private float observationPeriodEnergyStart; // Previous energy level
    private long observationPeriodStart;   // Total time elapsed
    private long observationPeriodEnd;   // Total time elapsed
    public float energyTimeLeftSec;

    // KeyCard
    public List<string> playerKeyCardColor;

    // XP
    public int currentXP;


    private void Start()
    {
        energyLeft = maxEnergy;

        optimalTemp = currentTemperature;

    }

    public void AddKeyCard(string keyCardName)
    {
        playerKeyCardColor.Add(keyCardName);      
    }

    public void ChangeTemp(float deltaTemp, float changeMinTemp, float changeMaxTemp) 
    {
        currentTemperature = currentTemperature + deltaTemp;
        minTemperature = minTemperature + changeMinTemp;
        maxTemperature = maxTemperature + changeMaxTemp;
    }

    IEnumerator DamageTicker(int damage)
    {
        yield return new WaitForSeconds(damageTicker);

        if(currentTemperature <= activationTemperature.x || currentTemperature >= activationTemperature.y)
        {
            GetComponent<PlayerManager>().TakeDamage(damage);
        }
    }

    public void ChangeEnergy(float deltaEnergy, float changeMaxEnergy, float tickEnergyFactor, float tickEnergyInterval)
    {
        if(GameObject.FindGameObjectWithTag("Menu").GetComponent<MenuController>().menuActive == false)
        {
            energyLeft = energyLeft + deltaEnergy;

            //EnergyClock(deltaEnergy, tickEnergyFactor, tickEnergyInterval);

            maxEnergy = maxEnergy + changeMaxEnergy;

            if (energyLeft >= maxEnergy)
            {
                energyLeft = maxEnergy;
            }
        }
        
    }

    public void ChangeBulletCount(int deltaBullets, int changeMaxBullets)
    {
        remainingBullets = remainingBullets + deltaBullets;
        maxBullets = maxBullets + changeMaxBullets;

        if(remainingBullets >= maxBullets)
        {
            remainingBullets = maxBullets;
        }
    }

    public void EnergyClock(float tickEnergyUsage, float tickEnergyFactor, float tickEnergyInterval)
    {
        energyLeft = energyLeft + tickEnergyUsage * tickEnergyFactor;

    }

    public void ChangeXP(int xp)
    {
        currentXP = currentXP + xp;
    }

    public void TemperatureRegulation()
    {
        if(isCold == true && currentTemperature != optimalTemp && energyLeft > 0)
        {
            Debug.Log("regulating Cold");

            currentTemperature = currentTemperature + regulationStrength;
            energyLeft = energyLeft - regulationEnergyUse;
        }
        if (isHot == true && currentTemperature != optimalTemp && energyLeft > 0)
        {
            Debug.Log("regulating Hot");

            currentTemperature = currentTemperature - regulationStrength;
            energyLeft = energyLeft - regulationEnergyUse;
        }
    }

    private void FixedUpdate()
    {
        activationTemperature.x = minTemperature - temperatureDeviation;
        activationTemperature.y = maxTemperature + temperatureDeviation;

        if (currentTemperature <= activationTemperature.x)
        {
            isCold = true;
            regulationOn = true;
        }
        else if (currentTemperature >= activationTemperature.y)
        {
            isHot = true;
            regulationOn = true;
        }
        else if (currentTemperature >= optimalTemp - 5f && currentTemperature <= optimalTemp + 5f)
        {
            isHot = false;
            isCold = false;
        }

        if (regulationOn == true)
        {
            TemperatureRegulation();
        }

        if (tickEnergyTimeDelta > timer)
        {
            timer = timer + 1;
            //Debug.Log("timer " + timer);
        }
        else if (tickEnergyTimeDelta == timer)
        {
            //Debug.Log("timer " + timer + "clock");
            timer = 0;
        }

        if (currentTemperature <= activationTemperature.x || currentTemperature >= activationTemperature.y)
        {
            Debug.Log("Temperature is too extreme!");
        }
        else if (energyLeft <= 0)
        {
            // Debug.Log("Energy depleted!");
            energyLeft = 0;
        }

        /*
        if (currentTemperature < activationTemperature.x + temperaturePanelDeviation)
        {
            // Calculate the adjusted value based on proximity to the maximum value (0.1)
            float adjustedValue = 1 - Mathf.InverseLerp(0f, activationTemperature.x, panelDeltaDeviation);
            // Ensure adjustedValue is within [0, 0.1]
            adjustedValue = Mathf.Clamp(adjustedValue, 0f, 0.1f);

            TemperaturePanel.GetComponent<Image>().color = new Color(0, 0.5f, 1, adjustedValue);
        }
        else if (currentTemperature > activationTemperature.y - temperaturePanelDeviation)
        {
            // Calculate the adjusted value based on proximity to the maximum value (0.1)
            float adjustedValue = 1 - Mathf.InverseLerp(0f, activationTemperature.y, panelDeltaDeviation);
            // Ensure adjustedValue is within [0, 0.1]
            adjustedValue = Mathf.Clamp(adjustedValue, 0f, 0.1f);

            TemperaturePanel.GetComponent<Image>().color = new Color(1, 0.5f, 0, adjustedValue);
        }
        */

        /*
        if (currentTemperature < activationTemperature.x + temperaturePanelDeviation)
        {
            // Calculate the adjusted value based on proximity to the minimum value (0)
            float adjustedValue = Mathf.InverseLerp(activationTemperature.x + temperaturePanelDeviation, activationTemperature.x, currentTemperature);
            adjustedValue = Mathf.Clamp(adjustedValue, 0f, 0.1f);

            TemperaturePanel.GetComponent<Image>().color = new Color(0, 0.5f, 1, adjustedValue);
        }
        else if (currentTemperature > activationTemperature.y - temperaturePanelDeviation)
        {
            // Calculate the adjusted value based on proximity to the maximum value (0.1)
            float adjustedValue = Mathf.InverseLerp(activationTemperature.y - temperaturePanelDeviation, activationTemperature.y, currentTemperature);
            adjustedValue = Mathf.Clamp(adjustedValue, 0f, 0.1f);

            TemperaturePanel.GetComponent<Image>().color = new Color(1, 0.5f, 0, adjustedValue);
        }
        else
        {
            TemperaturePanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        }



        if (currentTemperature + 2 < minTemperature)
        {
            currentTemperature = minTemperature - 3;
            StartCoroutine(DamageTicker(1));
            TemperaturePanel.GetComponent<Image>().color = new Color(0, 0, 1, 0.1f);
        }
        else if (currentTemperature - 2 > maxTemperature)
        {
            currentTemperature = maxTemperature + 3;
            StartCoroutine(DamageTicker(1));
            TemperaturePanel.GetComponent<Image>().color = new Color(1, 0, 0, 0.1f);
        }
        */

    }


    private void Update()
    {
        if(energyLeft < 0)
        {
            energyLeft = 0;
        }

        // Check if a certain time period has passed (e.g., 5 seconds)
        long nowInMilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        if (observationPeriodEnd <= nowInMilliseconds )
        {
            // calculate average change of energy for the defined period
            float frameDurationInSeconds= (observationPeriodEnd - observationPeriodStart) / 1000; 
            float averageEneryPerSecond = (observationPeriodEnergyStart - energyLeft) / frameDurationInSeconds;
            
            // reset the frame
            observationPeriodStart = nowInMilliseconds;
            observationPeriodEnd = observationPeriodStart + 1000;
            observationPeriodEnergyStart = energyLeft;

            // extrapolate the time left until energy get 0
            energyTimeLeftSec = energyLeft / averageEneryPerSecond;
            //Debug.Log(averageEneryPerSecond +" - "+ energyTimeLeftSec+ " : "+energyLeft);
        }

    }
}
