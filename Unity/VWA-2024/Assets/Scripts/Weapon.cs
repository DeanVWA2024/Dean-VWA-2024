using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //shooting
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    //orbiting around player
    float distance = 1f;                            // Distance of the weapon from the player
    float threshold = .1f;                          // Threshold distance before updating the weapon's position
    public float weaponSpeed = 10f;                 // Speed of the weapon's movement
    private Vector3 mousePos;

    //cooldown
    public float shootingCooldown = 0.5f;           // The time between shots
    private float shootingTimer = 0f;               // The timer for shooting cooldown

    //shooting ressources
    public float energyUsage;
    public int bulletUsage;
    public bool usesBullets;

    //Meele
    public bool isMeele;
    public float rotationSpeed = 5f;
    private Transform pivotPoint;
    Animator animtr;
    public bool isSwinging;
    public int meeleDamage;
    public float meeleKnockbackForce;
    public float meeleKnockbackDuration;
    Vector2 dirToPlayer;


    public DisplayStats displayStats;

   
    private void Start()
    {
        animtr = gameObject.GetComponentInParent<Animator>();
    }

    private void Update()
    {
        // Reduce cooldown timer
        if (shootingTimer > 0f)
        {
            shootingTimer -= Time.deltaTime;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isSwinging == true && collision.CompareTag("Enemy"))
        {
            //Debug.Log("Meele hit");
            dirToPlayer = transform.parent.position - transform.root.position;

            if(collision.gameObject.GetComponent<EnemyEnergyEater>() != null)
            {
                collision.gameObject.GetComponent<EnemyEnergyEater>().TakeDamage(meeleDamage);
            }
            else if (collision.gameObject.GetComponent<EnemyLaser>() != null)
            {
                collision.gameObject.GetComponent<EnemyLaser>().TakeDamage(meeleDamage);
            }
            else 
            {
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(meeleDamage);
                collision.gameObject.GetComponent<EnemyController>().Knockback(dirToPlayer.normalized, meeleKnockbackForce, meeleKnockbackDuration, true);
            }
            isSwinging = false;

            displayStats = collision.GetComponent<DisplayStats>();

            displayStats.ShowDamage(collision.transform.position, meeleDamage);

        }

    }


    public void Fire()
    {
        // GetComponentInParent<Ressources>().energyLeft > 0 || GetComponentInParent<Ressources>().remainingBullets > 0
        // change to differentiate between energy and bullet weapons!! maybe energy + bullets at the same time
        if (shootingTimer <= 0f)
        {
            if (usesBullets == true && GetComponentInParent<Ressources>().remainingBullets > 0 && isMeele != true)
            {
                // Instantiate the bullet prefab at the fire point position and rotation
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

                // Add force to the bullet rigidbody to make it move in the direction of fire
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
                }
                shootingTimer = shootingCooldown;

                GetComponentInParent<Ressources>().ChangeBulletCount(-bulletUsage, 0);

                bullet.GetComponent<BulletManager>().shooter = "Player";
            }

            else if (usesBullets == false && GetComponentInParent<Ressources>().energyLeft > 0 && isMeele != true)
            {
                // Instantiate the bullet prefab at the fire point position and rotation
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

                // Add force to the bullet rigidbody to make it move in the direction of fire
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(transform.right * bulletSpeed, ForceMode2D.Impulse);
                }
                shootingTimer = shootingCooldown;

                GetComponentInParent<Ressources>().ChangeEnergy(-energyUsage, 0, 0, 0);

                bullet.GetComponent<BulletManager>().shooter = "Player";
            }

            else if(isMeele == true)
            {
                isSwinging = true;

                //Debug.Log("shooting");

                //animtr.applyRootMotion = false;
                animtr.SetBool("swinging", true);

                /*
                if(animtr.GetBool("isUpright") == true)
                {
                    animtr.Play("SwordSwing");
                }
                else if(animtr.GetBool("isUpright") == false)
                {
                    animtr.Play("SwordSwingUpsideDown");
                }

                if (collision != null && collision.gameObject.tag == "Enemy")
                {
                    Debug.Log("Colliding with Player!");
                }
                */
                shootingTimer = shootingCooldown;

                animtr.Play("SwordSwing");


                //WaitForAnimation();

                animtr.SetBool("swinging", false);
                //animtr.applyRootMotion = true;
                animtr.StopPlayback();

            }



        }
        
    }

    IEnumerator WaitForAnimation()
    {
        //animtr.SetBool("swinging", true);
        //float animationLength = animtr.GetCurrentAnimatorStateInfo(0).length;
        //yield return new WaitForSecondsRealtime(animationLength);
        //animtr.SetBool("swinging", false);

        yield return new WaitForSecondsRealtime(1);
    }

    private void FixedUpdate()
    {
        //Debug.Log(animtr.GetBool("isUpright"));
        //Debug.Log(animtr.GetBool("swinging"));
        // Get the mouse position in world space
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Calculate the direction towards the mouse cursor
        Vector3 directionToCursor = (mousePos - transform.root.position).normalized;

        // Calculate the target position for the weapon
        Vector3 targetPos = transform.root.position + directionToCursor * distance;

        // Only update the weapon's position if the distance to the target is greater than a threshold
        if (Vector3.Distance(transform.parent.position, targetPos) > threshold && animtr.GetBool("swinging") != true)
        {
            // Move the weapon towards the target position
            transform.parent.position = Vector3.Lerp(transform.parent.position, targetPos, Time.deltaTime * weaponSpeed);

            // Rotate the weapon towards the mouse cursor
            float angle = Mathf.Atan2(directionToCursor.y, directionToCursor.x) * Mathf.Rad2Deg;
            transform.parent.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // Wrong hit direction
        // Flip the weapon if it passes the middle of the player rotation
        float angleDifference = Mathf.DeltaAngle(transform.parent.eulerAngles.z, transform.parent.parent.eulerAngles.z);
        if (angleDifference > 90f || angleDifference < -90f)
        {
            //animtr.SetBool("isUpright", false);
            /*
            Vector3 scale = transform.localScale;
            scale.y = -1f;
            transform.localScale = scale;
            */

            transform.parent.Rotate(-180, 0, 0);

            if (isMeele == true)
            {
                //transform.parent.Rotate(0, 180, 0);
                //scale.x = -1f;
                //transform.localScale = scale;
            }
        }
        else
        {
            //animtr.SetBool("isUpright", true);
            transform.parent.Rotate(0, 0, 0);

            /*
            Vector3 scale = transform.localScale;
            scale.y = 1f;
            transform.localScale = scale;
            if (isMeele == true)
            {
                //scale.x = 1f;
                transform.localScale = scale;
            }
            */
        }

    }
}


    