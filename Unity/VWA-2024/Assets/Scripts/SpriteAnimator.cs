using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] animationFrames; // Array of sprites representing the animation frames
    public float maxFramesPerSecond; // Maximum frames per second for the animation
    public float minFramesPerSecond; // Minimum frames per second for the animation

    public SpriteRenderer spriteRenderer;
    public int currentFrameIndex = 0;
    public float timeSinceLastFrame = 0f;
    public float framesPerSecond;

    public bool isOn;

    public NavMeshAgent2D agent;

    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animationFrames.Length == 0 || spriteRenderer == null)
        {
            Debug.LogError("Animation setup is incomplete. Make sure to assign frames and SpriteRenderer.");
        }
    }

    public virtual void Update()
    {
        if(agent != null)
        {
            // Get the agent's velocity (replace this with your own way of obtaining velocity)
            float agentVelocity = GetAgentVelocity();

            // Adjust frames per second based on velocity
            framesPerSecond = Mathf.Lerp(minFramesPerSecond, maxFramesPerSecond, agentVelocity);
        }
        else
        {
            framesPerSecond = maxFramesPerSecond;
        }
        
        

        // Calculate time per frame based on frames per second
        float timePerFrame = 1f / framesPerSecond;

        if (isOn == true)
        {
            // Increment time since the last frame
            timeSinceLastFrame += Time.deltaTime;

            // Check if it's time to switch to the next frame
            if (timeSinceLastFrame >= timePerFrame)
            {
                // Reset time since the last frame
                timeSinceLastFrame = 0f;

                // Update the sprite to the next frame
                UpdateSprite(animationFrames);
            }
        }
    }

    public virtual float GetAgentVelocity()
    {
        return agent.velocity.magnitude;
    }

    public virtual void UpdateSprite(Sprite[] frames)
    {
        // Increment the frame index
        currentFrameIndex++;

        // Loop back to the first frame if at the end
        if (currentFrameIndex >= animationFrames.Length)
        {
            currentFrameIndex = 0;
        }

        // Set the sprite based on the current frame index
        spriteRenderer.sprite = frames[currentFrameIndex];
    }
}

