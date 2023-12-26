using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Spawner class, spawns chosen enemies,
 * can block an entrance on the scene untill
 * all spawned enemies are dead*/
public class Spawner : MonoBehaviour
{
    // References
    public Enemy spawn;
    public GameObject barricade;
    // Variables
    public int spawnAmount = 0;
    public float spawnRate = 0;
    public float SensorRadius = 0;
    public float maxLeft, maxRight;
    public bool hasStartedSpawn = false;
    public bool rotateAtSpawn = false;
    private List<Enemy> spawnedEnemies = new List<Enemy>();

    // Update is called once per frame
    private void Update()
    {
        StartCoroutine(CheckIfToStartSpawn());
    }
    // If player is nearby, start spawning enemies
    private IEnumerator CheckIfToStartSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (!hasStartedSpawn)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, SensorRadius);

                foreach (Collider2D collider in colliders)
                {
                    if (collider.CompareTag("Player"))
                    {
                        StartCoroutine(SpawnCoroutine());
                        hasStartedSpawn = true;
                        break;
                    }
                    if (hasStartedSpawn == true)
                        break;
                }
            }
        }

    }
    // Spawn enemies
    private IEnumerator SpawnCoroutine()
    {
        for (int x = 1; x <= spawnAmount; x++)
        {
            Vector3 pos = transform.position;
            pos.y += 1;
            Enemy newEnemy = Instantiate(spawn, pos, Quaternion.identity);
            newEnemy.maxLeft = maxLeft;
            newEnemy.maxRight = maxRight;
            newEnemy.isAlive = true;
            newEnemy.fromSpawner = true;
            if (x % 2 == 0)
                newEnemy.moveRight = true;
            else
                newEnemy.moveRight = false;
            if (rotateAtSpawn)
                newEnemy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            spawnedEnemies.Add(newEnemy);

            yield return new WaitForSeconds(spawnRate);
        }
        StartCoroutine(CheckAllEnemiesDead());
    }
    // Check if all spawned enemies are dead
    private IEnumerator CheckAllEnemiesDead()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            bool allDead = true;

            foreach (Enemy enemy in spawnedEnemies)
            {
                if (enemy.isAlive)
                {
                    allDead = false;
                    break;
                }
            }
            if (allDead)
            {
                yield return new WaitForSeconds(2.0f);
                OpenGate();
                break;
            }
        }
    }
    // If the gate is active, remove it
    private void OpenGate()
    {
        for (int x = spawnedEnemies.Count - 1; x >= 0; x--)
        {
            if (!spawnedEnemies[x].isAlive)
            {
                Destroy(spawnedEnemies[x].gameObject);
                spawnedEnemies.RemoveAt(x);
            }
        }
        try
        {
            Vector3 startPosition = barricade.transform.position;
            StartCoroutine(RemoveBarricade());
        }
        catch { };
    }
    // Removing barricade
    private IEnumerator RemoveBarricade()
    {
        for (int x = 0; x < 2000; x++)
        {
            barricade.transform.position += new Vector3(0, -0.03f, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }
}

