using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ReactorAnimation : MonoBehaviour
{
    public bool isActivated;
    public bool activateOthers = false;
    public Sprite[] workingFrames; // Array of sprites representing the animation frames
    public Sprite[] animationFrames; // Array of sprites representing the animation frames
    public float maxFramesPerSecond; // Maximum frames per second for the animation
    public float minFramesPerSecond; // Minimum frames per second for the animation

    public SpriteRenderer spriteRenderer;
    public int currentFrameIndex = 0;
    public float timeSinceLastFrame = 0f;
    public float framesPerSecond;
    public bool hasStarted = false;


    public void Update()
    {
        if(activateOthers == true && hasStarted == false)
        {
            StartCoroutine(ChangeSprite(1000, 0.5f, workingFrames));
            hasStarted = true;
            GameObject.FindWithTag("LightManager").GetComponent<LightManager>().DisableLights("Emergency Lights");
            GameObject.FindWithTag("LightManager").GetComponent<LightManager>().EnableLights("Normal Lights");
            GameObject.FindWithTag("LightManager").GetComponent<LightManager>().EnableLights("Computer Lights");
            GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>().intensity = 0.7f;
        }
    }

    public void ActivateReactor()
    {
        if(isActivated == false)
        {
            StartCoroutine(ChangeSprite(animationFrames.Length - 1, 0.1f, animationFrames));
            isActivated = true;
        }
    }

    public IEnumerator ChangeSprite(int changeCount, float duration, Sprite[] frames)
    {
        UpdateSprite(frames);

        for (int i = 1; i < changeCount; i++)
        {
            yield return new WaitForSecondsRealtime(duration);
            UpdateSprite(frames);
        }
        Debug.Log("others activated");
        activateOthers = true;
    }

    public void UpdateSprite(Sprite[] frames)
    {
        // Increment the frame index
        currentFrameIndex++;

        // Loop back to the first frame if at the end
        if (currentFrameIndex >= frames.Length)
        {
            currentFrameIndex = 0;
        }

        // Set the sprite based on the current frame index
        spriteRenderer.sprite = frames[currentFrameIndex];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActivateReactor();
        Debug.Log("Reactor");
    }
}
