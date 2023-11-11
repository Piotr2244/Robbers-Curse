using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentScript : MonoBehaviour
{
    public Hero hero;

    //STATISTICS
    private Text goldText;
    private Text strengthText;
    private Text speedText;
    private Text jumpText;
    //SPELLS
    private SpriteRenderer spell1;
    private SpriteRenderer spell2;
    private SpriteRenderer spell3;
    private SpriteRenderer spell4;
    private SpriteRenderer spell5;

    //ITEMS
    public Item item1;
    public Item item2;
    public Item item3;
    public Item item4;

    private void Awake()
    {
        goldText = transform.Find("GoldBar/gold").GetComponent<Text>();
        strengthText = transform.Find("StrengthBar/strength").GetComponent<Text>();
        speedText = transform.Find("SpeedBar/speed").GetComponent<Text>();
        jumpText = transform.Find("JumpBar/jump").GetComponent<Text>();

        spell1 = transform.Find("SpellBar/1").GetComponent<SpriteRenderer>();
        spell2 = transform.Find("SpellBar/2").GetComponent<SpriteRenderer>();
        spell3 = transform.Find("SpellBar/3").GetComponent<SpriteRenderer>();
        spell4 = transform.Find("SpellBar/4").GetComponent<SpriteRenderer>();
        spell5 = transform.Find("SpellBar/5").GetComponent<SpriteRenderer>();

        item1 = transform.Find("Equipment/Eq1").GetComponent<Item>();
        item2 = transform.Find("Equipment/Eq2").GetComponent<Item>();
        item3 = transform.Find("Equipment/Eq3").GetComponent<Item>();
        item4 = transform.Find("Equipment/Eq4").GetComponent<Item>();

        ItemToCollect.OnItemCollision += GetItem;

    }

    private void Update()
    {
        if (hero != null)
        {
            goldText.text = hero.gold.ToString();
            strengthText.text = hero.damage.ToString();
            speedText.text = hero.speed.ToString();
            jumpText.text = hero.jumpForce.ToString();
            changeSpell();
            useItem();
        }
    }

    private void changeSpell()
    {
        spell1.enabled = false;
        spell2.enabled = false;
        spell3.enabled = false;
        spell4.enabled = false;
        spell5.enabled = false;
        switch (hero.spellIndex)
        {
            case 1:
                spell1.enabled = true;
                break;
            case 2:
                spell2.enabled = true;
                break;
            case 3:
                spell3.enabled = true;
                break;
            case 4:
                spell4.enabled = true;
                break;
            case 5:
                spell5.enabled = true;
                break;
            default:
                break;
        }
    }

    private void useItem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            item1.UseCurrentItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            item2.UseCurrentItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            item3.UseCurrentItem();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            item4.UseCurrentItem();
        }
    }

    private void GetItem(int indexx, GameObject instance)
    {
        if (item1.ItemIndex == 0)
            item1.ItemIndex = indexx;
        else if (item2.ItemIndex == 0)
            item2.ItemIndex = indexx;
        else if (item3.ItemIndex == 0)
            item3.ItemIndex = indexx;
        else if (item4.ItemIndex == 0)
            item4.ItemIndex = indexx;
        else
            return;
        Destroy(instance);

    }

    public bool BuyItem(int indexx)
    {
        if (item1.ItemIndex == 0)
        {
            item1.ItemIndex = indexx;
            return true;
        }
        else if (item2.ItemIndex == 0)
        {
            item2.ItemIndex = indexx;
            return true;
        }
        else if (item3.ItemIndex == 0)
        {
            item3.ItemIndex = indexx;
            return true;
        }
        else if (item4.ItemIndex == 0)
        {
            item4.ItemIndex = indexx;
            return true;
        }
        else
            return false;
    }
}
