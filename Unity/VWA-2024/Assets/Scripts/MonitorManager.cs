using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorManager : DoorOpener
{
    public string requiredKeyCardColor;
    GameObject door;
    public bool keyCardCheckedDoor;
    public bool hasTurnedOn;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        door = GameObject.FindGameObjectWithTag("Door");

        if(isOn == false)
        {
            hasTurnedOn = false;
        }
        else
        {
            hasTurnedOn = true;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (GameObject.FindGameObjectWithTag("Reactor").GetComponent<ReactorAnimation>().activateOthers == true)
        {
            isOn = true;
        }

        keyCardCheckedDoor = door.GetComponent<DoorOpener>().keyCardChecked;

        if(isOn == true && hasTurnedOn == false)
        {
            StartCoroutine(ChangeSprite(1, 0.1f, animationFrames));
            FindObjectOfType<AudioManager>().Play("boot");
            hasTurnedOn = true;
        }


        float distanceToTarget = Vector3.Distance(player.position, transform.position);

        // Check if the player is close enough
        if (distanceToTarget <= proximityDistance && player.GetComponent<Ressources>().playerKeyCardColor.Contains(requiredKeyCardColor) && keyCardCheckedDoor == false && isOn == true)
        {
            StartCoroutine(ChangeSprite(4, 0.5f, animationFrames));
            door.GetComponent<DoorOpener>().keyCardChecked = true;
            FindObjectOfType<AudioManager>().Play("accepted");
        }
    }
}
