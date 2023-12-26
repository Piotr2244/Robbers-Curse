using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Spawns falling fire balls */
public class FireRain : MonoBehaviour
{
    // Variables and references
    public float damage = 10f;
    private Animator animator;
    private AudioSource src;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(0, 0, -90));
        animator = GetComponent<Animator>();
        animator.SetBool("hit", false);
        src = transform.GetComponent<AudioSource>();

    }
    // Hurt enemy if he is in collision
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
    // Destroy spell after time if enemy or ground was hit
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
