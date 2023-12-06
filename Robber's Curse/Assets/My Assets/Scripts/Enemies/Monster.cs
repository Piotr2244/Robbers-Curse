using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Enemy
{
    public Monster()
    {
        speed = 0.5f;
        attackRange = 1.0f;
        health = 20.0f;
        attackSpeed = 1.0f;
        damage = 2.0f;
    }

    void Update()
    {
        animator.SetBool("Move", true);
        SmartEnemy();
    }
}
