using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/* Hero status holder, helps to brong
 * Stats between scenes */
public class HeroStatus : MonoBehaviour
{
    // References
    public Hero heroLoad;
    public EquipmentScript equipmentLoad;
    public HeroStateControl stateLoad;
    // Variables
    public bool isActive = true;
    //hero
    public float speed = 0f;
    public float jumpForce = 0f;
    public float Maxhealth = 0f;
    public float health = 0f;
    public float MaxMana = 0f;
    public float mana = 0f;
    public float toxic = 0f; //max is 100;
    public float damage = 0f;
    public float attackRange = 0f;
    public float gold = 0;
    public bool[] spellLearned = Enumerable.Repeat(false, 5).ToArray();
    //equipment
    public int item1;
    public int item2;
    public int item3;
    public int item4;
    //state
    public int currentFatigue = 1;
    public int currentInjuries = 1;
    public int currentSickness = 1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Save status before entering to a new scene
    public void SaveStatus()
    {
        speed = heroLoad.speed;
        jumpForce = heroLoad.jumpForce;
        Maxhealth = heroLoad.Maxhealth;
        health = heroLoad.health;
        MaxMana = heroLoad.MaxMana;
        mana = heroLoad.mana;
        toxic = heroLoad.toxic;
        damage = heroLoad.damage;
        attackRange = heroLoad.attackRange;
        gold = heroLoad.gold;
        spellLearned = heroLoad.spellLearned;

        item1 = equipmentLoad.item1.ItemIndex;
        item2 = equipmentLoad.item2.ItemIndex;
        item3 = equipmentLoad.item3.ItemIndex;
        item4 = equipmentLoad.item4.ItemIndex;

        currentFatigue = stateLoad.currentFatigue;
        currentInjuries = stateLoad.currentInjuries;
        currentSickness = stateLoad.currentSickness;
    }

}
