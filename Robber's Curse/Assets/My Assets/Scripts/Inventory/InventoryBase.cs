using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Inventory base, stores all available
 * items in game */

public class InventoryBase : MonoBehaviour
{
    // Variables and references
    public List<Sprite> sprites = new List<Sprite>();
    public List<string> texts = new List<string>();
    public Hero hero;

    // Items to use
    public delegate void UseItem();
    public UseItem[] ItemFunctions;
    // Start is called before the first frame update
    public void Start()
    {
        texts.Add(""); //INDEX 0
        texts.Add("Small healing potion"); //INDEX 1
        texts.Add("Healing potion");        //INDEX2
        texts.Add("Large healing potion");  //3
        texts.Add("Small mana potion");     //4
        texts.Add("Mana potion");           //5
        texts.Add("Large mana potion");     //6
        texts.Add("SpellBook");             //7

        ItemFunctions = new UseItem[]
            {
                Item0Used,
                Item1Used,
                Item2Used,
                Item3Used,
                Item4Used,
                Item5Used,
                Item6Used,
                Item7Used,
            };
    }

    // Use items after their activation
    public void Item0Used() { } //no item
    public void Item1Used()
    {
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
    public void Item7Used()
    {
        for (int x = 0; x < hero.spellLearned.Length; x++)
        {
            if (!hero.spellLearned[x])
            {
                hero.spellLearned[x] = true;
                break;
            }
        }
    }
}
