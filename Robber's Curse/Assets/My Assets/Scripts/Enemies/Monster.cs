using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Monster, follows the player if he
 * is in range*/
public class Monster : Enemy
{
    //Constructor
    public Monster()
    {
        speed = 0.5f;
        attackRange = 1.0f;
        health = 20.0f;
        attackSpeed = 1.0f;
        damage = 2.0f;
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Move", true);
        SmartEnemy();
    }
}
