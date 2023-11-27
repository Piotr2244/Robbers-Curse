using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    public float damage = 3.0f;
    private bool hit = false;
    private void Start()
    {
        StartCoroutine(destroyAfterTime());
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hit)
        {
            hit = true;
            other.GetComponent<Hero>().TakeDamage(damage);
        }
    }

    public IEnumerator destroyAfterTime()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(gameObject);
    }
}
