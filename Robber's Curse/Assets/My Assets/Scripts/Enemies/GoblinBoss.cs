using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBoss : Enemy
{
    public RockSpawner spawner;
    public bool isFighting = false;
    public GameObject barricade;
    public GameObject Closingbarricade;
    private bool isRemoving = false;
    private bool closingGate = false;
    public GoblinBoss()
    {
        speed = 1.0f;
        attackRange = 2.0f;
        health = 80.0f;
        attackSpeed = 1.0f;
        damage = 4.0f;
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Move", true);
        SmartEnemy();

        if (!isAlive)
        {
            if (!isRemoving)
                StartCoroutine(RemoveBarricade());
            animator.SetBool("Dead", true);
            spawner.spawn = false;
        }

        if (animator.GetBool("Stand") == false && !isFighting)
        {
            if (spawner != null)
                spawner.ForceStartSpawn();
            if (!closingGate)
                StartCoroutine(CloseGate());
        }
    }

    private IEnumerator RemoveBarricade()
    {
        isRemoving = true;
        for (int x = 0; x < 2000; x++)
        {
            if (barricade != null)
            {
                barricade.transform.position += new Vector3(0, -0.03f, 0);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator CloseGate()
    {
        closingGate = true;
        for (int x = 0; x < 200; x++)
        {
            if (Closingbarricade != null)
            {
                Closingbarricade.transform.position += new Vector3(0, 0.01f, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
