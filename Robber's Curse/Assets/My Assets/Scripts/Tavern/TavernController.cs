using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TavernController : MonoBehaviour
{
    public Hero hero;
    public EquipmentScript equipment;
    // HEALING
    private TextMeshProUGUI healPrice;
    private float healPriceAmount;
    //MORE MANA
    private TextMeshProUGUI manaPrice;
    private float manaPriceAmount;
    //SHOP
    private float Item1Price, Item2Price, Item3Price;
    private TextMeshProUGUI Item1DisplayPRice, Item2DisplayPRice, Item3DisplayPRice;
    public Item item1, item2, item3;
    void Start()
    {
        healPrice = transform.Find("HealButton/Text (TMP)").GetComponent<TextMeshProUGUI>();
        manaPrice = transform.Find("ManaButton/Text (TMP)").GetComponent<TextMeshProUGUI>();
        Item1DisplayPRice = transform.Find("ShopItem1/Price/Text (TMP)").GetComponent<TextMeshProUGUI>();
        Item2DisplayPRice = transform.Find("ShopItem2/Price/Text (TMP)").GetComponent<TextMeshProUGUI>();
        Item3DisplayPRice = transform.Find("ShopItem3/Price/Text (TMP)").GetComponent<TextMeshProUGUI>();
        item1 = transform.Find("ShopItem1").GetComponent<Item>();
        item2 = transform.Find("ShopItem2").GetComponent<Item>();
        item3 = transform.Find("ShopItem3").GetComponent<Item>();
        setPrices();
    }

    private void setPrices()
    {
        System.Random random = new System.Random();
        healPriceAmount = random.Next(3, 9);
        healPrice.text = "Heal price: " + healPriceAmount.ToString();
        manaPriceAmount = random.Next(30, 100);
        manaPrice.text = "More mana for: " + manaPriceAmount.ToString();
        RandomiseItems();

    }

    public void HealHero()
    {
        if (hero.gold >= healPriceAmount && hero.health < hero.Maxhealth)
        {
            hero.gold -= healPriceAmount;
            hero.health += 1;
        }
    }
    public void GetMoreMana()
    {
        if (hero.gold >= manaPriceAmount)
        {
            hero.gold -= manaPriceAmount;
            hero.MaxMana += 1;
        }
    }

    private void RandomiseItems()
    {
        int randomInt;
        System.Random random = new System.Random();
        randomInt = random.Next(1, 6);
        item1.ItemIndex = randomInt;
        Item1Price = randomInt * random.Next(20, 30);
        randomInt = random.Next(1, 6);
        item2.ItemIndex = randomInt;
        Item2Price = randomInt * random.Next(20, 30);
        randomInt = random.Next(1, 6);
        item3.ItemIndex = randomInt;
        Item3Price = randomInt * random.Next(20, 30);

        Item1DisplayPRice.text = "Buy for: " + Item1Price.ToString();
        Item2DisplayPRice.text = "Buy for: " + Item2Price.ToString();
        Item3DisplayPRice.text = "Buy for: " + Item3Price.ToString();
    }

    public void BuyItem1()
    {
        if (hero.gold >= Item1Price && item1.ItemIndex != 0)
        {
            if (equipment.BuyItem(item1.ItemIndex))
            {
                Item1DisplayPRice.text = "Sold!";
                item1.ItemIndex = 0;
                hero.gold -= Item1Price;
            }
        }
    }
    public void BuyItem2()
    {
        if (hero.gold >= Item2Price && item2.ItemIndex != 0)
        {
            if (equipment.BuyItem(item2.ItemIndex))
            {
                Item2DisplayPRice.text = "Sold!";
                item2.ItemIndex = 0;
                hero.gold -= Item2Price;
            }
        }
    }
    public void BuyItem3()
    {
        if (hero.gold >= Item3Price && item3.ItemIndex != 0)
        {
            if (equipment.BuyItem(item3.ItemIndex))
            {
                Item3DisplayPRice.text = "Sold!";
                item3.ItemIndex = 0;
                hero.gold -= Item3Price;
            }
        }
    }
}
