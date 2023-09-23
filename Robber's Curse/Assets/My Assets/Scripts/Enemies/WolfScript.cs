using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WolfScript : Enemy
{
    public WolfScript()
    {
        speed = 3.0f;
        attackRange = 1.0f;
        health = 10.0f;
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
