using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    // Player
    public GameObject player;
    public RectTransform rtHealth;
    public RectTransform rtEnergy;
    public RectTransform rtTemp;


    // Health
    public float healthScale;
    public int currentHealth;
    public int maxHealth;
    public GameObject HealthBar;
    float widthHealth;
    float heightHealth;

    // Health UI

    // Energy
    public float energyScale;
    public float currentEnergy;
    public float maxEnergy;
    public GameObject EnergyBar;
    float widthEnergy;
    float heightEnergy;

    // Energy UI
    public TextMeshProUGUI energyPercentageUI; // Reference to the TextMeshPro component
    public float energyPercentage;

    public TextMeshProUGUI energySecRemainingUI;
    public float energySecRemaining;

    // Bullets UI
    public TextMeshProUGUI bulletsLeftUI;
    public int bulletsLeft;


    // Temperature
    /*
    public float tempScale;
    public int currentTemp;
    public int maxTemp;
    public int minTemp;
    public GameObject TempBar;
    float widthTemp;
    float heightTemp;
    */
    // Bar with pointer??
    // public Slider temperatureSlider;
    // public RectTransform arrowRectTransform;
    //public Text temperatureText;

    public Sprite hotTemp;
    public Sprite coldTemp;
    public Sprite normalTemp;
    public Sprite hotArrow;
    public Sprite coldArrow;
    public Sprite noArrow;
    public GameObject Thermometer;
    public GameObject TemperatureArrow;
    private Color originalTempColor;

    public float sliderMax;
    public float tempScale;
    public float currentTemp;
    public float maxTemp;
    public float minTemp;
    float widthTemp;
    float heightTemp;

    public float timeInterval = 0.01f; // Time interval for tracking (in seconds)
    private float lastTemp;
    private float timer;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        lastTemp = currentTemp;

        //rtHealth = GetComponentInChildren<RectTransform>();
        originalTempColor = rtTemp.GetComponent<Image>().color;

        foreach (var rectTransform in GetComponentsInChildren<RectTransform>(true))
        {
            if (rectTransform.name == "Health")
            {
                rtHealth = rectTransform;
                widthHealth = rtHealth.sizeDelta.x;
                heightHealth = rtHealth.sizeDelta.y;
            }
            else if (rectTransform.name == "Energy")
            {
                rtEnergy = rectTransform;
                widthEnergy = rtEnergy.sizeDelta.x;
                heightEnergy = rtEnergy.sizeDelta.y;
            }
            else if (rectTransform.name == "Temperature")
            {
                /*
                rtTempBar = rectTransform;
                widthTemp = rtTempBar.sizeDelta.x * rtTempBar.localScale.x;
                heightTemp = rtTempBar.sizeDelta.y * rtTempBar.localScale.y;
                */
            }
        }
    }

    private void Update()
    {
        // Health
        currentHealth = player.GetComponent<PlayerManager>().currentHealth;
        maxHealth = player.GetComponent<PlayerManager>().maxHealth;

        healthScale = (float) currentHealth / (float) maxHealth;
        
        rtHealth.sizeDelta = new Vector2(widthHealth * healthScale, heightHealth);

        // Energy
        currentEnergy = player.GetComponent<Ressources>().energyLeft;
        maxEnergy = player.GetComponent<Ressources>().maxEnergy;

        energyScale = (float)currentEnergy / (float)maxEnergy;

        rtEnergy.sizeDelta = new Vector2(widthEnergy * energyScale, heightEnergy);

        // Bullets
        bulletsLeft = player.GetComponent<Ressources>().remainingBullets;

        // Temperature
        currentTemp = player.GetComponent<Ressources>().currentTemperature;
        maxTemp = player.GetComponent<Ressources>().maxTemperature;
        minTemp = player.GetComponent<Ressources>().minTemperature;

        tempScale = (float)currentTemp / (float)maxTemp;

        rtTemp.sizeDelta = new Vector2(widthEnergy, heightEnergy * tempScale);

        if(tempScale < 0.2f)
        {
            Thermometer.GetComponent<Image>().sprite = coldTemp;
            rtTemp.GetComponent<Image>().color = new Color ((float) 83/255,(float) 149/255,(float) 251/255);
        }
        else if (tempScale > 0.8f)
        {
            Thermometer.GetComponent<Image>().sprite = hotTemp;
        }
        else
        {
            Thermometer.GetComponent<Image>().sprite = normalTemp;
            rtTemp.GetComponent<Image>().color = originalTempColor;
        }
        // temperatureSlider.minValue = minTemp;
        // temperatureSlider.maxValue = maxTemp;

        // Update the timer
        timer += Time.deltaTime;

        // Check if the specified time interval has passed
        if (timer >= timeInterval)
        {
            // Calculate the change in value
            float valueChange = currentTemp - lastTemp;
            //Debug.Log("Timer " + valueChange);


            if (valueChange > 0)
            {
                TemperatureArrow.GetComponent<Image>().sprite = hotArrow;
                TemperatureArrow.GetComponent<RectTransform>().localScale = new Vector2(0.3f, 0.3f);
            }
            else if (valueChange < 0)
            {
                TemperatureArrow.GetComponent<Image>().sprite = coldArrow;
                TemperatureArrow.GetComponent<RectTransform>().localScale = new Vector2(0.3f, 0.3f);
            }
            else
            {
                TemperatureArrow.GetComponent<Image>().sprite = noArrow;
                TemperatureArrow.GetComponent<RectTransform>().localScale = new Vector2(0.3f, 0.15f);
            }

            timer = 0f;
            lastTemp = currentTemp;
        }

        // Update the UI to reflect the new temperature
        UpdateTemperatureUI();
        UpdateEnergyUI();
        UpdateBulletUI();
    }

    /*
    public void OnTemperatureChange(float deltaTemp)
    {
        // Update the arrow position based on the slider value
        float normalizedValue = deltaTemp / temperatureSlider.maxValue;
        float arrowPositionX = normalizedValue * temperatureSlider.GetComponent<RectTransform>().rect.width;

        // Set the arrow's new position
        Vector3 arrowPosition = arrowRectTransform.anchoredPosition;
        arrowPosition.x = arrowPositionX;
        arrowRectTransform.anchoredPosition = arrowPosition;
    }
    */

    void UpdateTemperatureUI()
    {
        // Update the temperature text
        //temperatureText.text = $"Temperature: {currentTemp:F2}°C";

        // Update the arrow position based on the current temperature
        // float normalizedTemperature = currentTemp / temperatureSlider.maxValue;
        // float arrowPositionX = normalizedTemperature * temperatureSlider.GetComponent<RectTransform>().rect.width;

        // max change factor = 0.65

        // Set the arrow's new position
        // Vector3 arrowPosition = arrowRectTransform.anchoredPosition;
        // arrowPosition.x = arrowPositionX * 0.65f;
        // arrowRectTransform.anchoredPosition = arrowPosition;
    }

    void UpdateEnergyUI()
    {
        energyPercentage = currentEnergy / maxEnergy * 100;
        //Debug.Log(energyPercentage);
        energyPercentageUI.text = (int) energyPercentage + "%";
        
        energySecRemaining = player.GetComponent<Ressources>().energyTimeLeftSec;
        if (energySecRemaining >= 120)
        {
            energySecRemainingUI.text = ">120 sec";
        }
        else if (currentEnergy <= 0)
        {
            energyPercentageUI.text = "empty"; 
        }
        else
        {
            energySecRemainingUI.text = (int)energySecRemaining + " sec";
        }
        
    }

    void UpdateBulletUI()
    {
        bulletsLeftUI.text = bulletsLeft.ToString();
    }
}
