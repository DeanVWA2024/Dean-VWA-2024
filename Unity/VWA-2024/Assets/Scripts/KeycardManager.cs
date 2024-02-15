using UnityEngine;
using System.Collections.Generic;

public class KeycardManager : ItemPickup
{
    public Sprite KeyCardColor;
    public string KeyCardName;
    private SpriteRenderer spriteRenderer;

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer.sprite = KeyCardColor;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnPickup()
    {
        // Perform pickup actions
        FindObjectOfType<AudioManager>().Play("powerUp");
        Debug.Log("Player picked up " + KeyCardName + " Keycard!");
        Player.GetComponent<Ressources>().AddKeyCard(KeyCardName);
        Destroy(gameObject); // or deactivate the item, etc.
    }



}



