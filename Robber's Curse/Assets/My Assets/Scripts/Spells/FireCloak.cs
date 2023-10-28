using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCloak : MonoBehaviour
{
    private GameObject player;
    private Hero hero;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hero = player.GetComponent<Hero>();
        StartCoroutine(Spell());
    }

    private void Update()
    {
        if (player != null)
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, transform.position.z);
    }

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
