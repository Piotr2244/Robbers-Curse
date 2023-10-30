using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernController : MonoBehaviour
{
    public Canvas textCanvas;
    public Hero hero;
    //HEALING
    private Text healPrice;
    private float healPriceAmount;
    //
    void Start()
    {
        textCanvas.gameObject.SetActive(true);
        healPrice = textCanvas.transform.Find("HealPrice").GetComponent<Text>();
        setHealPrice();
    }

    void Update()
    {

    }

    private void setHealPrice()
    {
        System.Random random = new System.Random();
        healPriceAmount = random.Next(3, 9);
        healPrice.text = "Heal price: " + healPriceAmount.ToString();
    }

    public void HealHero()
    {
        hero.gold -= healPriceAmount;
        hero.health += 2;
    }
}
