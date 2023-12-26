using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Spell spawning magic skulls under
 * the map to hurt enemies */
public class DeathSpell : MonoBehaviour
{
    // Variables
    public float damage = 10f;
    public float speed = 4f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyAfterSomeTime());
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
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
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
