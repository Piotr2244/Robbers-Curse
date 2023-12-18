using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WindSpell : MonoBehaviour
{
    public float damage = 2.0f;
    public float speed = 6f;
    private Transform playerTransform;
    Vector3 direction;

    private void Start()
    {
        StartCoroutine(destroyAfterSomeTime());
    }
    void Update()
    {
        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;

                if (playerTransform.localScale.x < 0)
                {
                    direction = Vector3.right;
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    direction = Vector3.left;
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
        }
        transform.Translate(direction * speed * Time.deltaTime);
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
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
