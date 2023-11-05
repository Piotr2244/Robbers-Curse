using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TavernController : MonoBehaviour
{
    public Hero hero;
    // HEALING
    private TextMeshProUGUI healPrice;
    private float healPriceAmount;
    //MORE MANA
    private TextMeshProUGUI manaPrice;
    private float manaPriceAmount;
    void Start()
    {
        healPrice = transform.Find("HealButton/Text (TMP)").GetComponent<TextMeshProUGUI>();
        manaPrice = transform.Find("ManaButton/Text (TMP)").GetComponent<TextMeshProUGUI>();
        setPrices();
    }

    void Update()
    {

    }

    private void setPrices()
    {
        System.Random random = new System.Random();
        healPriceAmount = random.Next(3, 9);
        healPrice.text = "Heal price: " + healPriceAmount.ToString();
        manaPriceAmount = random.Next(30, 100);
        manaPrice.text = "More mana for: " + manaPriceAmount.ToString();
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
}
