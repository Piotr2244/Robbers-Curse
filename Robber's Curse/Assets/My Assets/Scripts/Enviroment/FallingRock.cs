using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Falling rock, moves down and
 * hurts the player */
public class FallingRock : MonoBehaviour
{
    // Variables
    public float damage = 3.0f;
    private bool hit = false;
    // Start is called on scene load
    private void Start()
    {
        StartCoroutine(destroyAfterTime());
    }
    // Deal damage to player after collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hit)
        {
            hit = true;
            other.GetComponent<Hero>().TakeDamage(damage);
        }
    }
    // Remove object after time
    public IEnumerator destroyAfterTime()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(gameObject);
    }
}
