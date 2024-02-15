using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : SpriteAnimator
{
    public Transform player; // Assign the player's transform in the Inspector
    public BoxCollider2D boxCollider;
    public float proximityDistance = 5f; // Adjust this value based on how close you want the player to be
    private bool doorOpened = false;
    public bool requiresKeyCard;
    public bool keyCardChecked = false;
    public bool isntMonitor;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assign the player's transform in the Inspector
    }

    // Update is called once per frame
    public override void Update()
    {
        if(GameObject.FindGameObjectWithTag("Reactor").GetComponent<ReactorAnimation>().activateOthers == true)
        {
            isOn = true;
        }

        if (requiresKeyCard == false && isOn == true)
        {
            float distanceToTarget = Vector3.Distance(player.position, transform.position);

            if (distanceToTarget <= proximityDistance && doorOpened == false)
            {
                StartCoroutine(ChangeSprite(animationFrames.Length - 1, maxFramesPerSecond, animationFrames));
                doorOpened = true;
            }
        }
        else if (requiresKeyCard == true && keyCardChecked == true && doorOpened == false && isOn == true)
        {
            if(isntMonitor == true)
            {
                StartCoroutine(ChangeSprite(animationFrames.Length - 1, maxFramesPerSecond, animationFrames));
            }
            doorOpened = true;
        }
    }
    
    public virtual IEnumerator ChangeSprite(int changeCount, float duration, Sprite[] frames)
    {
        UpdateSprite(frames);

        for(int i = 1; i < changeCount; i++)
        {
            yield return new WaitForSecondsRealtime(duration);
            UpdateSprite(frames);
        }

        if(boxCollider != null)
        {
            boxCollider.enabled = false;
        }
        
    }
}
