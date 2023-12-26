using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * UI bars showing current hp, mana or toxic level */
public class BarsController : MonoBehaviour
{
    // Variables
    public GameObject image;
    public Hero hero;
    private Vector3 tempPos;
    // References
    public int index; // where to show the bar
    public int TypeIndex; // 1=health, 2=mana, 3=toxic

    // Update is called once per frame
    void Update()
    {
        Vector3 newScale = image.transform.localScale;
        if (TypeIndex == 1)
            newScale.x = hero.health / hero.Maxhealth;
        if (TypeIndex == 2)
            newScale.x = hero.mana / hero.MaxMana;
        if (TypeIndex == 3)
            newScale.x = hero.toxic / 100;

        image.transform.localScale = newScale;
    }
    // Late update is called later than update
    void LateUpdate()
    {
        tempPos = transform.position;
        if (index == 1)
        {
            tempPos.x = hero.transform.position.x - (5.25f);
            tempPos.y = hero.transform.position.y + (4.0f);
        }
        if (index == 2)
        {
            tempPos.x = hero.transform.position.x + (1.25f);
            tempPos.y = hero.transform.position.y + (4.0f);
        }
        if (index == 3)
        {
            tempPos.x = hero.transform.position.x + (8.0f);
            tempPos.y = hero.transform.position.y + (4.0f);
        }
        transform.position = tempPos;
    }
}
