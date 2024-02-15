using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChestManager : MonoBehaviour
{
    public GameObject itemToDrop;
    public float pickupRadius = 2f;
    private bool isOpened = false;
    public Sprite[] animationFrames; // Array of sprites representing the animation frames
    public SpriteRenderer spriteRenderer;
    public int spriteIndex = 0;

    private void Start()
    {
        spriteRenderer.sprite = animationFrames[spriteIndex];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpened && (other.CompareTag("Player") || other.CompareTag("Bullet") || other.CompareTag("Sword")))
        {
            // Perform chest opening actions
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpened = true;
        spriteIndex++;
        spriteRenderer.sprite = animationFrames[spriteIndex];

        // Drop the item at the chest's position
        GameObject droppedItem = Instantiate(itemToDrop, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.identity);

        // Attach a script to the dropped item to handle pickup
        ItemPickup itemPickupScript = droppedItem.AddComponent<ItemPickup>();
        itemPickupScript.SetPickupRadius(pickupRadius);

        //gameObject.SetActive(false);
    }


}

