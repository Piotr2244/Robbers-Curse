using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public List<string> texts = new List<string>();

    public Hero hero;

    //items to use
    public delegate void UseItem();
    public UseItem[] ItemFunctions;
    public void Start()
    {
        texts.Add(""); //INDEX 0
        texts.Add("Small healing potion"); //INDEX 1
        texts.Add("Healing potion");        //INDEX2
        texts.Add("Large healing potion");  //3
        texts.Add("Small mana potion");     //4
        texts.Add("Mana potion");           //5
        texts.Add("Large mana potion");     //6

        ItemFunctions = new UseItem[]
            {
                Item0Used,
                Item1Used,
                Item2Used,
                Item3Used,
                Item4Used,
                Item5Used,
                Item6Used,
            };
    }

    public void Item0Used() { } //no item

    public void Item1Used()
    {
        Debug.Log("3");
        if (hero.health + 5 <= hero.Maxhealth)
            hero.health += 5;
        else
            hero.health = hero.Maxhealth;

    }
    public void Item2Used()
    {
        if (hero.health + 10 <= hero.Maxhealth)
            hero.health += 10;
        else
            hero.health = hero.Maxhealth;
    }
    public void Item3Used()
    {
        if (hero.health + 20 <= hero.Maxhealth)
            hero.health += 20;
        else
            hero.health = hero.Maxhealth;
    }

    public void Item4Used()
    {
        if (hero.mana + 10 <= hero.MaxMana)
            hero.mana += 10;
        else
            hero.mana = hero.MaxMana;
    }
    public void Item5Used()
    {
        if (hero.mana + 20 <= hero.MaxMana)
            hero.mana += 20;
        else
            hero.mana = hero.MaxMana;
    }
    public void Item6Used()
    {
        if (hero.mana + 50 <= hero.MaxMana)
            hero.mana += 50;
        else
            hero.mana = hero.MaxMana;
    }
}
