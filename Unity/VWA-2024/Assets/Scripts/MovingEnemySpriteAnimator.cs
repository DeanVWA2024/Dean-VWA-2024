using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemySpriteAnimator : MonoBehaviour
{
    public Sprite[] stationaryFrames;
    public int stationaryFPS;
    public Sprite[] startingMovementFrames;
    public int movingFPS;
    public Sprite[] maxSpeedFrames;
    public int maxFPS;

    private SpriteRenderer spriteRenderer;
    private int currentFrameIndex = 0;
    private float timeSinceLastFrame = 0f;
    private float framesPerSecond;

    public float startingMovementThreshold;
    public float timeBetweenStationary = 2f;
    public float timeBetweenStartingMovement = 1f;
    public float timeBetweenMaxSpeed = 0.5f;

    public NavMeshAgent2D agent;

    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (stationaryFrames.Length == 0 || spriteRenderer == null)
        {
            Debug.LogError("Animation setup is incomplete. Make sure to assign frames and SpriteRenderer.");
        }
    }

    public virtual void Update()
    {
        if (agent != null)
        {
            // Get the agent's velocity (replace this with your own way of obtaining velocity)
            float agentVelocity = GetAgentVelocity();

            // Choose frames based on agent's velocity
            if (agentVelocity == 0)
            {
                framesPerSecond = stationaryFPS;
                UpdateFrames(stationaryFrames, timeBetweenStationary);
            }
            else if (agentVelocity <= startingMovementThreshold)
            {
                framesPerSecond = movingFPS;
                UpdateFrames(startingMovementFrames, timeBetweenStartingMovement);
            }
            else
            {
                framesPerSecond = maxFPS;
                UpdateFrames(maxSpeedFrames, timeBetweenMaxSpeed);
            }
        }
        else
        {
            framesPerSecond = stationaryFPS;
            UpdateFrames(stationaryFrames, timeBetweenStationary);
        }
    }

    public virtual float GetAgentVelocity()
    {
        return agent.velocity.magnitude;
    }

    public virtual void UpdateFrames(Sprite[] frames, float timeBetweenFrames)
    {
        // Calculate time per frame based on frames per second
        float timePerFrame = 1f / framesPerSecond;

        // Increment time since the last frame
        timeSinceLastFrame += Time.deltaTime;

        // Check if it's time to switch to the next frame
        if (timeSinceLastFrame >= timePerFrame)
        {
            // Reset time since the last frame
            timeSinceLastFrame = 0f;

            // Set the sprite based on the current frame index
            spriteRenderer.sprite = frames[currentFrameIndex];

            // Increment the frame index
            currentFrameIndex = (currentFrameIndex + 1) % frames.Length;
        }
    }
}

/*
public class MovingEnemySpriteAnimator : MonoBehaviour
{
    public Sprite[] stationaryFrames;
    public int stationaryFPS;
    public Sprite[] startingMovementFrames;
    public int movingFPS;
    public Sprite[] maxSpeedFrames;
    public int maxFPS;

    private SpriteRenderer spriteRenderer;
    private int currentFrameIndex = 0;
    private float timeSinceLastFrame = 0f;
    float framesPerSecond;
    public float startingMovementThreshold;

    public NavMeshAgent2D agent;

    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (stationaryFrames.Length == 0 || spriteRenderer == null)
        {
            Debug.LogError("Animation setup is incomplete. Make sure to assign frames and SpriteRenderer.");
        }
    }

    public virtual void Update()
    {
        if (agent != null)
        {
            // Get the agent's velocity (replace this with your own way of obtaining velocity)
            float agentVelocity = GetAgentVelocity();

            // Choose frames based on agent's velocity
            if (agentVelocity == 0)
            {
                framesPerSecond = stationaryFPS;
                UpdateFrames(stationaryFrames);
            }
            else if (agentVelocity <= startingMovementThreshold)
            {
                framesPerSecond = movingFPS;
                UpdateFrames(startingMovementFrames);
            }
            else
            {
                framesPerSecond = maxFPS;
                UpdateFrames(maxSpeedFrames);
            }
        }
        else
        {
            framesPerSecond = stationaryFPS;
            UpdateFrames(stationaryFrames);
        }

        // Calculate time per frame based on frames per second
        float timePerFrame = 1f / framesPerSecond;

        // Increment time since the last frame
        timeSinceLastFrame += Time.deltaTime;

        // Check if it's time to switch to the next frame
        if (timeSinceLastFrame >= timePerFrame)
        {
            // Reset time since the last frame
            timeSinceLastFrame = 0f;
        }
    }

    public virtual float GetAgentVelocity()
    {
        return agent.velocity.magnitude;
    }

    public virtual void UpdateFrames(Sprite[] frames)
    {
        // Reset the frame index
        currentFrameIndex = 0;

        // Set the sprite based on the current frame index
        spriteRenderer.sprite = frames[currentFrameIndex];

        currentFrameIndex++;
    }
}
*/