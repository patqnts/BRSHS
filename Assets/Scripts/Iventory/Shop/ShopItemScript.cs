using MoreMountains.InventoryEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemScript : MonoBehaviour
{
    public Button buyButton;
    public Image imageSprite;
    public TextMeshProUGUI itemName;
    public InventoryItem Item;
   
  
    //protected virtual void Start()
    //{
    //    Initialization();
    //}

    /// <summary>
    /// On Init we look for our target inventory
    /// </summary>
    public void InitializationOfItems()
    {
        //FindTargetInventory(Item.TargetInventoryName);
        imageSprite.sprite = Item.Icon;
        itemName.text = Item.ItemName;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
    }

    private void BuyItem()
    {
        var coins = FindObjectOfType<PlayerScript>().Coins;
        bool hasBuyButton = true;

        if (coins < Item.Price)
        {
            hasBuyButton = false;
        }

        
        ShopManagerScript.instance.ShowNotificationDialogueBox(Item, hasBuyButton);
    }
}
