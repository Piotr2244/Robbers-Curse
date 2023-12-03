using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static TavernController;

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

    private TextMeshProUGUI SleepButtonText, MedicButtonText, BeerButtonText;
    bool medicUsed = false, sleepUsed = false, beerUsed = false;
    //TRAINING
    private bool strTrained = false, speedTrained = false, jumpTrained = false;
    public TextMeshProUGUI TrainStrText, TrainSpeedText, TrainJumpText;

    //DELEGATES
    public delegate void SendStateUpdate(int newFatigue, int newInjuries, int newSickness, int mode);
    public static event SendStateUpdate UpdateState;
    public delegate void ChangeTrack(int index = 2);
    public static event ChangeTrack ChangeMusic;
    public delegate void RestoreTrack();
    public static event RestoreTrack RestoreMusic;
    void Start()
    {
        TrainStrText = transform.Find("TrainStr/Text (TMP)").GetComponent<TextMeshProUGUI>();
        TrainSpeedText = transform.Find("TrainSpeed/Text (TMP)").GetComponent<TextMeshProUGUI>();
        TrainJumpText = transform.Find("TrainJump/Text (TMP)").GetComponent<TextMeshProUGUI>();

        SleepButtonText = transform.Find("SleepButton/Text (TMP)").GetComponent<TextMeshProUGUI>();
        MedicButtonText = transform.Find("MedicButton/Text (TMP)").GetComponent<TextMeshProUGUI>();
        BeerButtonText = transform.Find("BeerButton/Text (TMP)").GetComponent<TextMeshProUGUI>();

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
    private void OnEnable()
    {
        ChangeMusic(2);
    }
    private void OnDisable()
    {
        RestoreMusic();
    }
    private void setPrices()
    {
        System.Random random = new System.Random();
        healPriceAmount = random.Next(3, 9);
        healPrice.text = "Heal price: " + healPriceAmount.ToString();
        manaPriceAmount = random.Next(10, 50);
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
        float modify = 1;
        System.Random random = new System.Random();
        randomInt = random.Next(1, 6);
        item1.ItemIndex = randomInt;
        if (randomInt <= 3)
            modify = 1;
        if (randomInt > 3 && randomInt < 7)
            modify = 0.3f;
        Item1Price = randomInt * modify * random.Next(20, 30);
        Item1Price = Mathf.Round(Item1Price);

        randomInt = random.Next(1, 6);
        item2.ItemIndex = randomInt;
        if (randomInt <= 3)
            modify = 1;
        if (randomInt > 3 && randomInt < 7)
            modify = 0.3f;
        Item2Price = randomInt * modify * random.Next(20, 30);
        Item2Price = Mathf.Round(Item2Price);

        randomInt = random.Next(1, 6);
        item3.ItemIndex = randomInt;
        if (randomInt <= 3)
            modify = 1;
        if (randomInt > 3 && randomInt < 7)
            modify = 0.3f;
        Item3Price = randomInt * modify * random.Next(20, 30);
        Item3Price = Mathf.Round(Item3Price);

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

    public void TrainStr()
    {
        if (!strTrained && hero.gold >= 200.0f)
        {
            hero.damage += 1.0f;
            hero.gold -= 200.0f;
            strTrained = true;
            TrainStrText.text = "Done";
        }
    }
    public void TrainSpeed()
    {
        if (!speedTrained && hero.gold >= 100.0f)
        {
            hero.speed += 1.0f;
            speedTrained = true;
            hero.gold -= 100.0f;
            TrainSpeedText.text = "Done";
        }
    }
    public void TrainJump()
    {
        if (!jumpTrained && hero.gold >= 150.0f)
        {
            hero.jumpForce += 1.0f;
            hero.gold -= 150.0f;
            jumpTrained = true;
            TrainJumpText.text = "Done";
        }
    }

    public void Sleep()
    {
        if (!sleepUsed && hero.gold >= 40.0f)
        {
            hero.gold -= 40.0f;
            sleepUsed = true;
            SleepButtonText.text = "Done";
            UpdateState(10, 0, 0, 3);
        }
    }
    public void Medic()
    {
        if (!medicUsed && hero.gold >= 50.0f)
        {
            hero.gold -= 50.0f;
            medicUsed = true;
            MedicButtonText.text = "Done";
            UpdateState(0, 10, 0, 3);
        }
    }
    public void Beer()
    {
        if (!beerUsed && hero.gold >= 30.0f)
        {
            hero.gold -= 30.0f;
            beerUsed = true;
            BeerButtonText.text = "Done";
            System.Random random = new System.Random();
            int x = random.Next(1, 15);
            Debug.Log(x);
            switch (x)
            {
                case 6:
                    UpdateState(0, 5, 0, 3);
                    break;
                case 7:
                    UpdateState(5, 0, 0, 3);
                    break;
                case 8:
                    UpdateState(0, 0, 2, 3);
                    break;
                case 9:
                    UpdateState(0, 0, 2, 2);
                    break;
                case 10:
                    UpdateState(5, 5, 0, 3);
                    break;
                case 11:
                    UpdateState(5, 0, 0, 2);
                    break;
                case 12:
                    hero.damage += 0.5f;
                    break;
                case 13:
                    hero.speed += 0.5f;
                    break;
                case 14:
                    hero.jumpForce += 0.5f;
                    break;
                case 15:
                    hero.jumpForce += 0.2f;
                    hero.damage += 0.2f;
                    hero.speed += 0.2f;
                    UpdateState(5, 5, 0, 3);
                    UpdateState(0, 0, 0, 2);
                    break;
                default:
                    break;
            }


        }
    }
}
