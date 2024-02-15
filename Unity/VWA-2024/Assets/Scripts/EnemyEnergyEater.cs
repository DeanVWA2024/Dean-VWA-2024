using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class EnemyEnergyEater : MonoBehaviour
{
    public float energyConsume;
    public float energyConsumeInterval;
    GameObject player;

    public int currentHealth;
    public int maxHealth;
    public bool isDead = false;

    public int experiencePoints;

    public void Start()
    {
        gameObject.SetActive(true);
        player = GameObject.FindWithTag("Player");
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

            GetComponent<DisplayStats>().UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    private void Die()
    {
        DisableChildLightsByScriptType<Light2D>();

        int xpAmount = 1;
        if (xpAmount == 1)
        {
            FindObjectOfType<AudioManager>().Play("die");

            player.GetComponent<Ressources>().ChangeXP(experiencePoints);
            xpAmount--;
        }

        isDead = true;
        transform.localScale = new Vector3(0, 0, 0);
        StartCoroutine(WaitForDeath());

        // Handle enemy death behavior here
        // Play death animation, drop loot, etc.
        // Award experience points to the player
    }

    // Wait for Text to disappear
    IEnumerator WaitForDeath()
    {
        yield return new WaitForSecondsRealtime(3f);
        Destroy(gameObject);
    }
    // Update is called once per frame
    public void Update()
    {

        player.GetComponent<Ressources>().ChangeEnergy(energyConsume, 0, 0.01f ,energyConsumeInterval);

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
