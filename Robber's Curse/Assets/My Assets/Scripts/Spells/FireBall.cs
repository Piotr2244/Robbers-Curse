using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Single fire spell to hurt enemies */
public class FireBall : MonoBehaviour
{
    // Variables and references
    public float damage = 7f;
    public float speed = 5f;
    private Transform playerTransform;
    Vector3 direction;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("hit", false);
        StartCoroutine(perish());
    }
    // Update is called once per frame
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
    // Hurt enemy if he is in collision
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
    // Destroy spell after time if enemy was hit
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    // Destroy spell after time if it is still active
    private IEnumerator perish()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }
}
