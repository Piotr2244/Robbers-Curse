using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSpell : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 4f;
    void Start()
    {
        StartCoroutine(destroyAfterSomeTime());
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    private IEnumerator destroyAfterSomeTime()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
