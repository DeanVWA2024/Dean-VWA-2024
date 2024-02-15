using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class EnemyDuplicator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;  // Time between spawns
    public int maxDuplicates = 5;     // Maximum number of duplicates
    public float timer;
    public float spawnOffset = 2f;    // Offset between spawned enemies

    public void Start()
    {
        gameObject.SetActive(true);
    }

    
    // Update is called once per frame
    public void Update()
    {

        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to spawn a new enemy
        if (timer >= spawnInterval)
        {
            timer = 0f;

            // Check if the maximum number of duplicates has not been reached
            if (transform.childCount < maxDuplicates && gameObject != null)
            {
                SpawnEnemy();
                Debug.Log("try spawn");
            }
        }

        if(enemyPrefab == null)
        {
            Destroy(gameObject);
        }

    }

    void SpawnEnemy()
    {

        // Find a valid position on the NavMesh
        Vector3 randomPosition = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

        // Instantiate a new enemy at the spawner's position
        GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

        // Set the new enemy as a child of the spawner
        newEnemy.transform.parent = transform;
    }
}


