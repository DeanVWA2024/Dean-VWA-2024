using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Health related variables
    public int maxHealth = 10;
    public int currentHealth;
    private bool isDead = false;
    public float healInterval = 2f;    // Time between healing intervals
    public int healAmount = 1;      // Amount of healing per interval
    private float healTimer;

    public Rigidbody2D rb;
    public SpriteRenderer spi;

    private Vector2 moveDirection;
    public float moveSpeed = 5f;

    private Vector2 knockbackDirection;
    private float knockbackSpeed;
    //public float maxMoveSpeed = 5f;
    //public float accelerationTime = .01f;
    public float currentSpeed;

    //public Material NormalColor;
    //public Material SlimeColor;



    private void Start()
    {
        currentHealth = maxHealth;
        rb = gameObject.GetComponent<Rigidbody2D>();
        spi = GetComponent<SpriteRenderer>();
        currentSpeed = moveSpeed;
    }


    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");             // Get the horizontal input value (-1 or 1)
        float verticalInput = Input.GetAxisRaw("Vertical");                 // Get the vertical input value (-1 or 1)

        moveDirection = new Vector2(horizontalInput, verticalInput).normalized;     // Normalize the input direction vector

        //float targetSpeed = moveDirection.magnitude * maxMoveSpeed;         // Calculate the target speed based on input direction and max speed
        //currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocity.x, accelerationTime);  // Smoothly interpolate between current and target speed in the x-axis

        // Update the timer
        healTimer += Time.deltaTime;

        // Check if it's time to apply healing
        if (healTimer >= healInterval)
        {
            healTimer = 0f;

            if (currentHealth <= maxHealth)
            {
                currentHealth = currentHealth + healAmount;

            }
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
        }



    }

    void FixedUpdate()
    {
        rb.velocity = (moveDirection * currentSpeed + knockbackDirection * knockbackSpeed);

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            spi.flipX = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            spi.flipX = false;
        }
    }

    public void SlowDownPlayer(float slowDownFactor, float slowDownDuration)
    {
        StartCoroutine(SlowdownCoroutine(slowDownFactor, slowDownDuration)); // Slowdown for 2 seconds with 50% speed reduction
    }

    private IEnumerator SlowdownCoroutine(float slowdownFactor, float duration)
    {
        // Apply slowdown effect
        currentSpeed *= slowdownFactor;

        spi.material.SetColor("_Color", new Color(0.635f, 1f, 0.58f, 1f));

        yield return new WaitForSeconds(duration - 0.2f);



        // Restore the original speed
        currentSpeed = moveSpeed;

        spi.material.SetColor("_Color", Color.white);


    }

    /*
    public void Knockback(Vector2 direction, float force, float duration, bool knocked)
    {
        Debug.Log("dir" + direction + " for" + force);
        Debug.Log("Player being knocked");

        knockbackDirection = direction;
        knockbackSpeed = force;
    }
    */


    public void Knockback(Vector2 direction, float force, float duration, bool knocked)
    {
        //Debug.Log("force" + force + "duration" + duration);

        if (knocked == true)
        {
            StartCoroutine(KnockbackCoroutine(direction, force, duration));
        }
    }

    // Coroutine to allow for smooth animations afterwards
    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        float timer = 0f;


        while (timer < duration)
        {
            timer += Time.deltaTime;
            knockbackDirection = direction;
            knockbackSpeed = force;
            // Apply knockback force


            yield return null;

            knockbackDirection = Vector2.zero;
            knockbackSpeed = 0;
        }

    }


    public void TakeDamage(int damageAmount)
    {
        //currentHealth = maxHealth; in Start()

        if (!isDead)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Im dead");
        FindObjectOfType<AudioManager>().Play("hitEnemy");

        GameObject.FindGameObjectWithTag("DeathScreen").GetComponent<DeathScreenController>().ShowDeathScreen();

        // Handle enemy death behavior here
        // Play death animation, drop loot, etc.
        // Award experience points to the player
    }

    
}
