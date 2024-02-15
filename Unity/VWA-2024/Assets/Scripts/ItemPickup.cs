using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public float pickupRadius = 2f;
    public GameObject Player;
    public int healingAmount;
    public int energyAmount;
    public int bulletAmount;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public virtual void Update()
    {
        // Check if the player is in the pickup radius
        if (Vector3.Distance(transform.position, Player.transform.position) <= pickupRadius && gameObject != null)
        {
            OnPickup();
        }
    }

    public virtual void OnPickup()
    {
        Player.GetComponent<PlayerManager>().TakeDamage(-healingAmount);
        Player.GetComponent<Ressources>().ChangeEnergy(energyAmount, 0, 0, 0);
        Player.GetComponent<Ressources>().ChangeBulletCount(bulletAmount, 0);

        FindObjectOfType<AudioManager>().Play("powerUp");

        // Perform pickup actions
        // Debug.Log("Player picked up the item!");
        Destroy(gameObject); // or deactivate the item, etc.
    }

    public void SetPickupRadius(float radius)
    {
        pickupRadius = radius;
    }
    
}

