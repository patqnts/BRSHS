using MoreMountains.InventoryEngine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemScript : MonoBehaviour
{
    public Button buyButton;
    public Image imageSprite;
    public InventoryItem Item;
    protected Inventory _targetInventory;
    protected int _pickedQuantity = 0;
    public int Quantity = 1;
    public bool PickableIfInventoryIsFull = false;
    protected virtual void Start()
    {
        Initialization();
    }

    /// <summary>
    /// On Init we look for our target inventory
    /// </summary>
    protected virtual void Initialization()
    {
        FindTargetInventory(Item.TargetInventoryName);
        imageSprite.sprite = Item.Icon;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);
    }

    private void BuyItem()
    {
        string playerID = "Player1";
     
        Pick(Item.TargetInventoryName, playerID);
    }

    public virtual void Pick(string targetInventoryName, string playerID = "Player1")
    {
        FindTargetInventory(targetInventoryName, playerID);
        if (_targetInventory == null)
        {
            return;
        }

        if (!Pickable())
        {
           // PickFail();
            return;
        }

        DetermineMaxQuantity();
        if (!Application.isPlaying)
        {
            if (!Item.ForceSlotIndex)
            {
                _targetInventory.AddItem(Item, 1);
            }
            else
            {
                _targetInventory.AddItemAt(Item, 1, Item.TargetIndex);
            }
        }
        else
        {
            MMInventoryEvent.Trigger(MMInventoryEventType.Pick, null, Item.TargetInventoryName, Item, _pickedQuantity, 0, playerID);
        }
        if (Item.Pick(playerID))
        {
            Quantity = Quantity - _pickedQuantity;
            //PickSuccess();
            //DisableObjectIfNeeded();
        }
    }

    protected virtual void DetermineMaxQuantity()
    {
        _pickedQuantity = _targetInventory.NumberOfStackableSlots(Item.ItemID, Item.MaximumStack);
        if (Quantity < _pickedQuantity)
        {
            _pickedQuantity = Quantity;
        }
    }
    public virtual bool Pickable()
    {
        if (!PickableIfInventoryIsFull && _targetInventory.NumberOfFreeSlots == 0)
        {
            // we make sure that there isn't a place where we could store it
            int spaceAvailable = 0;
            List<int> list = _targetInventory.InventoryContains(Item.ItemID);
            if (list.Count > 0)
            {
                foreach (int index in list)
                {
                    spaceAvailable += (Item.MaximumStack - _targetInventory.Content[index].Quantity);
                }
            }

            if (Item.Quantity <= spaceAvailable)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    public virtual void FindTargetInventory(string targetInventoryName, string playerID = "Player1")
    {
        _targetInventory = null;
        if (targetInventoryName == null)
        {
            return;
        }
        _targetInventory = Inventory.FindInventory(targetInventoryName, playerID);
    }
}
