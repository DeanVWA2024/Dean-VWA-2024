using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float moveSpeed = 5f;
    //public float maxMoveSpeed = 5f;
    //public float accelerationTime = .01f;
    //public float currentSpeed;      
    private Vector3 velocity;
    SpriteRenderer spi;

    private Rigidbody2D rb;
    private Vector2 moveDirection;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spi = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");             // Get the horizontal input value (-1 or 1)
        float verticalInput = Input.GetAxisRaw("Vertical");                 // Get the vertical input value (-1 or 1)

        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;     // Normalize the input direction vector

        //float targetSpeed = moveDirection.magnitude * maxMoveSpeed;         // Calculate the target speed based on input direction and max speed
        //currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocity.x, accelerationTime);  // Smoothly interpolate between current and target speed in the x-axis

    }

    void FixedUpdate()
    {
        rb.velocity = (moveDirection * moveSpeed);

        if (Input.GetAxisRaw("Horizontal") < 0)
        {  
            spi.flipX = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            spi.flipX = false;
        }
    }
}

