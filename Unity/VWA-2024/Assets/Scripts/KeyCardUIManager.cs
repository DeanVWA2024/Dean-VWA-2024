using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeyCardUIManager : MonoBehaviour
{
    public GameObject[] keyCardsUI;
    public PlayerManager playerManager;
    public Sprite redSprite;
    public Sprite greenSprite;
    public Sprite blueSprite;
    public Sprite purpleSprite;
    public Sprite emptySprite;

    public Dictionary<string, Sprite> spritesByColor = new Dictionary<string, Sprite>();


    private void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        // Exclude the first element by using Skip(1) (the parent GameObject)
        keyCardsUI = GetComponentsInChildren<Transform>().Skip(1).Select(t => t.gameObject).ToArray();
        foreach(GameObject cardObject in keyCardsUI)
        {
            cardObject.GetComponent<Image>().sprite = emptySprite;
        }
    }

    private void Update()
    {
        List<string> cardColors = playerManager.GetComponent<Ressources>().playerKeyCardColor;
        
        int maxItems = Math.Min(cardColors.Count, keyCardsUI.Length);
        for (int i = 0; i < maxItems; i++)
        {
            keyCardsUI[i].GetComponent<Image>().sprite = findSprite(cardColors[i]);
        }
    }


    private Sprite findSprite(string color) {
        switch (color)
        {
            case "Red":
                return redSprite;
            case "Green":
                return greenSprite;
            case "Blue":
                return blueSprite;
            case "Purple":
                return purpleSprite;
            default:
                return emptySprite;
        }
    }
}
