using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Goblin, simple enemy*/
public class GoblinScript : Enemy
{
    //Constructor
    public GoblinScript()
    {
        speed = 4.0f;
        attackRange = 0.8f;
        health = 5.0f;
        attackSpeed = 1.0f;
        damage = 1.0f;
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Move", true);
        CasualEnemy();
        if (!isAlive)
            animator.SetBool("Dead", true);
    }
}
