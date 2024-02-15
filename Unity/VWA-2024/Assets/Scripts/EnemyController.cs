using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;


public class EnemyController : MonoBehaviour
{

    // Health related variables
    public int maxHealth = 10;
    public int currentHealth;
    private bool isDead = false;


    // Combat related variables
    public int attackDamage;
    public float attackRange = 2f;
    public float attackCooldown = .5f;
    private bool canAttack = true; // A flag to track if the enemy can currently attack
    private float attackTimer; // Timer to track the time since the last attack
    public float knockbackForce;
    public float knockbackDuration;
    public Vector2 hitDir;

    public bool slowDownPlayer;
    public float slowDownFactor;
    public float slowDownDuration;


    // Movement related variables
    public float sightRange = 10f; // Range within which the enemy can see the player
    public NavMeshAgent2D agent;
    public SpriteRenderer enemySpriteRenderer; // Reference to the SpriteRenderer component of the enemy's sprite
    public Transform[] waypoints; // Array of patrol waypoints
    private int currentWaypointIndex = 0;
    public Rigidbody2D rb;
    private bool isKnockedBack = false;
    public Vector2 knockbackPos;


    // SeePlayer related variables
    public float detectionRadius = 5f;
    public float followDistance = 10;
    public LayerMask playerLayer;
    private bool hasSeenPlayer;
    public Vector2 lastPlayerPosition;
    public bool wasHit;

    //see if enemy can see player & is in moverange --> go & flip in move direction


    // Player related variables
    public Transform playerTransform;
    public float distanceToPlayer;
    public PlayerManager playerManager;


    // Experience points rewarded to the player on defeating the enemy
    public int experiencePoints = 50;

    public GameObject itemToDrop;
    public float pickupRadius = 2f;
    private AudioSource audioSource;
    private bool hasPlayed = false;


    public virtual void Start()
    {
        currentHealth = maxHealth;
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        audioSource = GetComponent<AudioSource>();

    }

    public virtual void Update()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        LookAtTarget();

        if(agent != null)
        {
            // Check if the character is moving
            if (agent.velocity != Vector2.zero)
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
        }
        
    }

    bool PlayerInLineOfSight()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;


        // Check for obstacles using Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRadius, playerLayer);

        // If there are no obstacles, the player is in line of sight
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            hasSeenPlayer = true;
            return true;
        }
        else
        {
            // Player is not in line of sight
            return false;
        }

    }

    public bool CanSeePlayer()
    {
        // Check if the player is within the detection radius
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        // If the player is detected
        if (playerCollider != null)
        {

            // Check if the player is in the enemy's line of sight
            if (PlayerInLineOfSight())
            {
                // Player is in line of sight
                //Debug.Log("Player in sight!");
                return true;
            }
            else
            {
                // Player is not in line of sight
                return false;
            }
        }

        else if (hasSeenPlayer == true && Vector2.Distance(transform.position, playerTransform.position) < followDistance)
        {
            return true;
        }

        else if (hasSeenPlayer == true && Vector2.Distance(transform.position, playerTransform.position) > followDistance)
        {
            // Player is not in the follow and detection radius
            hasSeenPlayer = false;
            lastPlayerPosition = playerTransform.position;
            return false;
        }

        else
        {
            hasSeenPlayer = false;
            return false;
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
                if (!hasPlayed)
                {
                    // Debug.Log("playing sound");
                    FindObjectOfType<AudioManager>().Play("die");
                    Die();
                    hasPlayed = true;
                }
            }

            GetComponent<DisplayStats>().UpdateHealthBar(currentHealth, maxHealth);
            //GetComponent<DisplayStats>().ShowDamage(transform.position, damageAmount);
        }
    }

    private void DropItem()
    {
        // Instantiate the item at the enemy's position
        GameObject droppedItem = Instantiate(itemToDrop, transform.position, Quaternion.identity);

        // Attach a script to the dropped item to handle pickup
        //ItemPickup itemPickupScript = droppedItem.AddComponent<ItemPickup>();
        //itemPickupScript.SetPickupRadius(pickupRadius);
    }
    
    private void Die()
    {
        DisableChildLightsByScriptType<Light2D>();

        int dropAmount = 1;
        if(dropAmount == 1 && itemToDrop != null)
        {
            DropItem();
            dropAmount--;
        }

        int xpAmount = 1;
        if (xpAmount == 1)
        {
            playerManager.GetComponent<Ressources>().ChangeXP(experiencePoints);
            xpAmount--;
        }

        isDead = true;
        transform.localScale = new Vector3(0, 0, 0);

        

        StartCoroutine(WaitForDeath());

        Destroy(gameObject, 2f);


        // Handle enemy death behavior here
        // Play death animation, drop loot, etc.
        // Award experience points to the player
    }

    // Wait for Text to disappear
    IEnumerator WaitForDeath()
    {
        if (GetComponentInChildren<LineRenderer>() != null)
        {
            Destroy(GetComponentInChildren<LineRenderer>());
        }

        yield return new WaitForSecondsRealtime(3f);

        Destroy(gameObject);
    }

    public void Attack()
    {
        if (isDead == false)
        {
            //Debug.Log("attack yes");
            distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // If the enemy can attack and the player is within attack range and the attack timer has passed the cooldown duration
            if (canAttack && distanceToPlayer <= attackRange && Time.time >= attackTimer && CanSeePlayer() == true)
            {
                //Debug.Log("will attack");
                // Perform the attack
                PerformAttack(hitDir, knockbackForce, knockbackDuration, attackDamage, slowDownPlayer, slowDownFactor, slowDownDuration);

                // Set the attack timer for the next attack
                attackTimer = Time.time + attackCooldown;
            }
            // Check if the player is within attack range
            // If yes, deal damage to the player
            // Apply attack cooldown
        }

    }

    public virtual void PerformAttack(Vector2 direction, float knockbackForce, float knockbackDuration, int damageAmount, bool slowDownPlayer, float slowDownFactor, float slowDownDuration)
    {
        playerManager.Knockback(direction, knockbackForce, knockbackDuration, true);
        playerManager.TakeDamage(damageAmount);

        if (slowDownPlayer == true)
        {
            Debug.Log("want to change color");
            playerManager.SlowDownPlayer(slowDownFactor, slowDownDuration);
        }



        // Add attack logic here, such as dealing damage to the player or triggering attack animations

        // Disable attack until cooldown is over
        //canAttack = false;

        // Call a method after cooldown duration to enable attack again
        //Invoke("EnableAttack", attackCooldown);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            wasHit = true;
            lastPlayerPosition = playerTransform.position;
            MoveTowardsTarget();
        }
    }

    //NavMesh Agent
    public void MoveTowardsTarget()
    {
        //Debug.Log("was hit " + wasHit);
        //Start() --> agent = GetComponent<NavMeshAgent>();
        //Update() --> MoveTowardsTarget(playerTransform.position);
        //check if is being knocked

        if (CanSeePlayer() == true)
        {
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);
        }
        else if (CanSeePlayer() == false && lastPlayerPosition != Vector2.zero)
        {
            agent.SetDestination(lastPlayerPosition);
        }
        else if (wasHit == true)
        {
            agent.SetDestination(lastPlayerPosition);
            Debug.Log("remDis " + agent.remainingDistance + " stopDis " + agent.stoppingDistance);
            if (agent.remainingDistance <= agent.stoppingDistance || CanSeePlayer() == true)
            {
                // Set the next waypoint as the destination
                wasHit = false;
                lastPlayerPosition = Vector2.zero;
                agent.isStopped = true;
            }
        }
        else
        {
            lastPlayerPosition = Vector2.zero;
            agent.isStopped = true;
        }

    }


    
    private void OnDrawGizmosSelected()
    {
        // Draw the detection radius and field of view angle in the Unity Editor for visual reference
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 lineOfSightStart = transform.position;
        Vector3 lineOfSightEnd = transform.position + (playerTransform.position - transform.position).normalized * followDistance;
        Gizmos.DrawLine(lineOfSightStart, lineOfSightEnd);

    }
    

    public void Knockback(Vector2 direction, float force, float duration, bool knocked)
    {
        isKnockedBack = knocked;

        if (isKnockedBack && agent != null)
        {
            agent.velocity = direction * force;//Knocks the enemy back when appropriate 
        }
        StartCoroutine(KnockBack(duration));
    }

    IEnumerator KnockBack(float duration)
    {
        isKnockedBack = true;

        yield return new WaitForSeconds(duration); //Only knock the enemy back for a short time     

        isKnockedBack = false;

    }


    /* Knockback isnt working together with navmesh agent, stuck if navmesh agent deactivated and then activated again
    public void Knockback(Vector2 direction, float force, float duration, bool knocked)
    {

        //agent.isStopped = true;

        //agent.enabled = false;
        //isKnockedBack = knocked;
        //rb.velocity = Vector2.zero;
        //agent.ResetPath();
        isKnockedBack = true;
        Debug.Log("force" + force + "duration" + duration);

        if (knocked == true)
        {
            StartCoroutine(KnockbackCoroutine(direction, force, duration));
        }
        
    }
    
    // Coroutine to allow for smooth animations afterwards
    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        float timer = 0f;

        // Apply knockback force
        //rb.AddForce(direction * force, ForceMode2D.Impulse);
        agent.SetDestination(new Vector2(transform.position.x, transform.position.y) + direction * force);

        //Debug.Log(rb.velocity);
        Debug.Log("Knockback happened");
        
        while (timer < duration)
        {
            timer += Time.deltaTime;
            Debug.Log("timer" + timer);
            yield return null;
        }

        isKnockedBack = false;
        //agent.enabled = true;
        //agent.isStopped = false;
   


        //agent.ResetPath();
        //agent.SetDestination(playerTransform.position);
        
    }
    */
    public void SetNextWaypoint()
    {
        //Start() --> agent = GetComponent<NavMeshAgent>(); & SetNextWaypoint();
        //Update()

        //Check if the enemy has reached the current waypoint
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            // Set the next waypoint as the destination
            SetNextWaypoint();
        }
        

        // Move to the next waypoint in the array (looping)
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    public void LookAtTarget()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;

        // Compare the x direction of the player relative to the enemy
        if (directionToPlayer.x < 0)
        {
            // Player is on the left side of the enemy, flip the sprite to face left
            enemySpriteRenderer.flipX = false;
        }
        else
        {
            // Player is on the right side of the enemy, flip the sprite to face right
            enemySpriteRenderer.flipX = true;
        }
    }


    void DisableChildLightsByScriptType<T>() where T : MonoBehaviour
    {
        // Get all child objects
        Transform[] childTransforms = GetComponentsInChildren<Transform>(true);

        foreach (Transform childTransform in childTransforms)
        {
            // Check if the child has the specified script component
            T scriptComponent = childTransform.GetComponent<T>();

            if (scriptComponent != null)
            {
                // Disable the script component (e.g., 2D light)
                scriptComponent.enabled = false;
            }
        }
    }

}
