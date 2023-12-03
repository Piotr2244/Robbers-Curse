using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRain : MonoBehaviour
{
    public float damage = 10f;
    private Animator animator;
    private AudioSource src;
    void Start()
    {
        transform.Rotate(new Vector3(0, 0, -90));
        animator = GetComponent<Animator>();
        animator.SetBool("hit", false);
        src = transform.GetComponent<AudioSource>();

    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(damage);
            animator.SetBool("hit", true);
            StartCoroutine(DestroyAfterDelay());
        }
        if (other.CompareTag("Enviroment"))
        {
            animator.SetBool("hit", true);
            StartCoroutine(DestroyAfterDelay());
        }
    }
    private IEnumerator DestroyAfterDelay()
    {
        src.Play();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            yield return new WaitForSeconds(0.03f);
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
