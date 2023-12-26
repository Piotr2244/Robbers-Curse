using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Coin script, can be collected to increase
 * hero gold amount */
public class Coin : MonoBehaviour
{
    // Variables and references
    public float value;
    private Rigidbody2D rb;
    // Events and delegates
    public delegate void Collect(Coin instance);
    public static event Collect OnItemCollision;

    // Start is called before the first frame update
    private void Start()
    {
        System.Random random = new System.Random();
        value = random.Next(3, 5);
        rb = GetComponent<Rigidbody2D>();
        JumpOnStart();
    }
    // Perform jump on start
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
    // Get collected by the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (OnItemCollision != null)
                OnItemCollision.Invoke(this);
        }
    }

}
