using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float damage = 10f;
    public float speed = 5f;
    private Transform playerTransform;
    Vector3 direction;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("hit", false);
        StartCoroutine(perish());
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
            animator.SetBool("hit", true);
            speed = 1;
            StartCoroutine(DestroyAfterDelay());

        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private IEnumerator perish()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }
}
