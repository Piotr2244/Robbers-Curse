using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Vulture, simple enemy*/
public class VultureScript : Enemy
{
    // Constructor
    public VultureScript()
    {
        speed = 3.0f;
        attackRange = 0.6f;
        health = 6.0f;
        attackSpeed = 0.7f;
        damage = 0.5f;
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
