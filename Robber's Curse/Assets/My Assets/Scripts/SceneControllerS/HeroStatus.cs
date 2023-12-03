using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroStatus : MonoBehaviour
{
    public bool isActive = true;

    //public Hero heroSave;
    //public EquipmentScript equipmentSave;
    //public HeroStateControl stateSave;
    public Hero heroLoad;
    public EquipmentScript equipmentLoad;
    public HeroStateControl stateLoad;

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

    public void SaveStatus()
    {
        // Zapis wartoœci do obiektów Save
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

        item1 = equipmentLoad.item1.ItemIndex;
        item2 = equipmentLoad.item2.ItemIndex;
        item3 = equipmentLoad.item3.ItemIndex;
        item4 = equipmentLoad.item4.ItemIndex;

        currentFatigue = stateLoad.currentFatigue;
        currentInjuries = stateLoad.currentInjuries;
        currentSickness = stateLoad.currentSickness;
    }

    //public void LoadStatus()
    //{
    //    heroLoad.speed = speed;
    //    heroLoad.jumpForce = jumpForce;
    //    heroLoad.Maxhealth = Maxhealth;
    //    heroLoad.health = health;
    //    heroLoad.MaxMana = MaxMana;
    //    heroLoad.mana = mana;
    //    heroLoad.toxic = toxic;
    //    heroLoad.damage = damage;
    //    heroLoad.attackRange = attackRange;
    //    heroLoad.gold = gold;

    //    equipmentLoad.item1 = item1;
    //    equipmentLoad.item2 = item2;
    //    equipmentLoad.item3 = item3;
    //    equipmentLoad.item4 = item4;

    //    stateLoad.currentFatigue = currentFatigue;
    //    stateLoad.currentInjuries = currentInjuries;
    //    stateLoad.currentSickness = currentSickness;
    //}
}
