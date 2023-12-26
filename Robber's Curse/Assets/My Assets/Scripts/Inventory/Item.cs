using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
/* Single item */
public class Item : MonoBehaviour
{
    // References
    public InventoryBase invBase;
    private TextMeshProUGUI itemName;
    private SpriteRenderer ItemSprite;

    public int ItemIndex = 0; // Indexes are shown in InventoryBase.cs
    // Awake is called at the begining
    private void Awake()
    {
        try
        {
            itemName = transform.Find("Image/Item/Text (TMP)").GetComponent<TextMeshProUGUI>();
            ItemSprite = transform.Find("Image/Item/sprite").GetComponent<SpriteRenderer>();
        }
        catch
        {
            Debug.Log("Index out");
        }
    }
    // Update is called once per frame
    private void Update()
    {
        itemName.text = invBase.texts[ItemIndex];
        try
        {
            ItemSprite.sprite = invBase.sprites[ItemIndex];
        }
        catch { }
    }
    // Use current item after activation
    public void UseCurrentItem()
    {
        if (ItemIndex != 0)
        {
            try
            {
                invBase.ItemFunctions[ItemIndex]();
                ItemIndex = 0;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }
}