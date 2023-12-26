using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Cloak that empowers hero for a while */
public class FireCloak : MonoBehaviour
{
    // References
    private GameObject player;
    private Hero hero;
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hero = player.GetComponent<Hero>();
        StartCoroutine(Spell());
    }
    // Update is called once per frame
    private void Update()
    {
        if (player != null)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, transform.position.z);
    }
    // Empower hero for a moment, then undo it
    public IEnumerator Spell()
    {
        hero.damage += 2;
        hero.speed += 3;
        hero.jumpForce += 2;
        yield return new WaitForSeconds(10f);
        hero.damage -= 2;
        hero.speed -= 3;
        hero.jumpForce -= 2;
        Destroy(gameObject);
    }
}
