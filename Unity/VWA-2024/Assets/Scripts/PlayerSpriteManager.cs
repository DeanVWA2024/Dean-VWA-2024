using System;
using System.Reflection;
using UnityEngine;

public class PlayerSpriteManager : MonoBehaviour
{
    public Vector2 movementDir;
    public float movementSpeed;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public Sprite[] idleSprites;   // Sprites for when the player is not moving
    public Sprite[] moveUpSprites;  // Sprites for upward movement
    public Sprite[] moveDownSprites;  // Sprites for downward movement
    public Sprite[] moveSideSprites;  // Sprites for side movement

    private Vector2 lastMovementDir = Vector2.down; // Default direction when not moving

    private AudioSource audioSource;
    private bool isMoving;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();

        // Check if AudioSource is attached
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing!");
        }
    }

    void Update()
    {
        // Check if the character is moving
        if (isMoving)
        {
            // Play the audio if it's not already playing
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Stop the audio if the character is not moving
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }

        movementDir = rb.velocity.normalized;
        movementSpeed = rb.velocity.magnitude;

        UpdateSprite();
    }


    void UpdateSprite()
    {
        if (movementDir != Vector2.zero)
        {
            isMoving = true;
            if (Mathf.Abs(movementDir.x) > Mathf.Abs(movementDir.y))
            {
                // Movement on x axis
                spriteRenderer.flipX = movementDir.x < 0;
                ChangeSprite(moveSideSprites);
                lastMovementDir = movementDir;
            }
            else
            {
                // Movement on y axis
                ChangeSprite(movementDir.y > 0 ? moveUpSprites : moveDownSprites);
                lastMovementDir = movementDir;
            }
        }
        else
        {
            isMoving = false;
            if (lastMovementDir.y > 0)
            {
                spriteRenderer.sprite = idleSprites[0];
            }
            else if (lastMovementDir.y < 0)
            {
                spriteRenderer.sprite = idleSprites[1];
            }
            else
            {
                spriteRenderer.sprite = idleSprites[2];
            }
        }
    }


    void ChangeSprite(Sprite[] sprites)
    {
        int index = Mathf.FloorToInt(Time.time * 5) % sprites.Length; // Change sprite every 0.2 seconds (adjust as needed)
        spriteRenderer.sprite = sprites[index];
    }
}
