using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Enemy spawn;
    public int spawnAmount = 0;
    public float spawnRate = 0;
    public float SensorRadius = 0;
    public GameObject barricade;
    public float maxLeft, maxRight;
    public bool hasStartedSpawn = false;

    private List<Enemy> spawnedEnemies = new List<Enemy>();

    private void Update()
    {
        StartCoroutine(CheckIfToStartSpawn());
    }


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
            spawnedEnemies.Add(newEnemy);

            yield return new WaitForSeconds(spawnRate);
        }
        StartCoroutine(CheckAllEnemiesDead());
    }

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

    private IEnumerator RemoveBarricade()
    {
        for (int x = 0; x < 2000; x++)
        {
            barricade.transform.position += new Vector3(0, -0.03f, 0);
            yield return new WaitForSeconds(0.1f);
        }
    }
}

