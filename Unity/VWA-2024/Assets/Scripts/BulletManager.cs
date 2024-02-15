using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public int damageAmount; // The amount of damage inflicted by the bullet
    public float knockbackForce;
    public float knockbackDuration;
    public string shooter;
    private Component controller;
    public Rigidbody2D rb;
    public Vector2 movementDir;

    public DisplayStats displayStats;

    private void Start()
    {
        /*
        knockbackForce = 1f;
        knockbackDuration = 1f;
        damageAmount = 1;
        */

        

    }

    private void Update()
    {
        movementDir = rb.velocity.normalized;
        float angle = Mathf.Atan2(movementDir.y, movementDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(shooter);
        Debug.Log(rb.velocity);
        displayStats = collision.GetComponent<DisplayStats>();



        // differentiate between bullet shot from player and enemy
        if (collision.gameObject.CompareTag("Player") && shooter == "Enemy")
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(damageAmount);
            collision.gameObject.GetComponent<PlayerManager>().Knockback(rb.velocity.normalized, knockbackForce, knockbackDuration, true);

        }
        else if (collision.gameObject.CompareTag("Enemy") && shooter == "Player")
        {
            if (collision.gameObject.GetComponent<EnemyEnergyEater>() != null)
            {
                collision.gameObject.GetComponent<EnemyEnergyEater>().TakeDamage(damageAmount);
            }
            else if (collision.gameObject.GetComponent<EnemyLaser>() != null)
            {
                collision.gameObject.GetComponent<EnemyLaser>().TakeDamage(damageAmount);
            }
            else
            {
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(damageAmount);
                collision.gameObject.GetComponent<EnemyController>().Knockback(rb.velocity.normalized, knockbackForce, knockbackDuration, true);
            }
     

            displayStats = collision.GetComponent<DisplayStats>();
            displayStats.ShowDamage(collision.transform.position, damageAmount);
        }



        // Check if the collided object has a damageable component
        //Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        //EnemyMover enemyMover = collision.gameObject.GetComponent<EnemyMover>();

        //if (damageable != null)
        //{
        // Call the damageable component's TakeDamage function and pass the damage amount
        //damageable.TakeDamage(damageAmount);

        /*
        Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
        enemyMover.Knockback(knockbackDirection, knockbackForce, knockbackDuration, true);
        */
        //}

        Destroy(gameObject);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

    }
}


