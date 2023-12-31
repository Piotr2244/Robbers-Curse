using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Rock spawner, spawns falling rocks */
public class RockSpawner : MonoBehaviour
{
    // References
    public FallingRock rock;
    // Variables
    public int maxLeft = 0;
    public int maxRight = 0;
    public float spawnSpeed = 5; //second delay between spawn
    public float sensorRadius = 1.0f;
    public bool spawn = true;
    private bool isSpawning = false;
    // Update is called once per frame
    void Start()
    {
        if (spawn)
            StartCoroutine(Spawner());
    }
    // Start spawning
    public void ForceStartSpawn()
    {
        spawn = true;
        if (!isSpawning)
        {
            StartCoroutine(Spawner());
            isSpawning = true;
        }
    }
    // Spawn rocks around
    public IEnumerator Spawner()
    {
        float randomX;
        System.Random random = new System.Random();
        while (true)
        {
            yield return new WaitForSeconds(spawnSpeed);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, sensorRadius);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    randomX = random.Next(maxLeft, maxRight);
                    Vector3 SpawnerPosition = transform.position;
                    SpawnerPosition.x += randomX;
                    randomX = random.Next(20, 25);
                    Instantiate(rock, SpawnerPosition, Quaternion.identity);
                }
            }
        }

    }
}
