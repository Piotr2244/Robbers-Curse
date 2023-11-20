using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float value;
    public delegate void Collect(Coin instance);
    public static event Collect OnItemCollision;
    private Rigidbody2D rb;

    private void Start()
    {
        System.Random random = new System.Random();
        value = random.Next(1, 5);
        rb = GetComponent<Rigidbody2D>();
        JumpOnStart();
    }
    void JumpOnStart()
    {
        int direction = UnityEngine.Random.Range(0, 2);
        int actualDirection;

        if (direction == 0)
            actualDirection = -1;
        else
            actualDirection = 1;

        Vector2 jumpVector = new Vector2(actualDirection * value, 2f);
        rb.velocity = jumpVector;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (OnItemCollision != null)
                OnItemCollision.Invoke(this);
        }
    }

}
