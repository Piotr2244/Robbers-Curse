using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
/* weak but fast spell harming multiple enemies */
public class WindSpell : MonoBehaviour
{
    // Variables and references
    public float damage = 2.0f;
    public float speed = 6f;
    private Transform playerTransform;
    Vector3 direction;
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(destroyAfterSomeTime());
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
        }
    }
    // Destroy spell after time
    private IEnumerator destroyAfterSomeTime()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
