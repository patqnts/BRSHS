using MoreMountains.InventoryEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    public static ShopManagerScript instance;
    // Start is called before the first frame update
    public InventoryItem[] itemList;
    public GameObject shopItemPrefab;
    public GameObject notificationDialogueBox;
    public Transform shopItemContainer;
    public Button buyItemButton;
    public Button closeDialogueBoxButton;
    public Button confirmDialogueBoxButton;
    public TextMeshProUGUI dialogueMessageText;

    private PlayerScript playerScript;
    protected int _pickedQuantity = 0;
    public int Quantity = 1;
    public bool PickableIfInventoryIsFull = false;
    protected Inventory _targetInventory;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerScript = FindFirstObjectByType<PlayerScript>();
        InitializeLoadItems();
    }
    public void InitializeLoadItems()
    {
        foreach (var item in itemList)
        {
            GameObject shopItem = Instantiate(shopItemPrefab, shopItemContainer);
            shopItem.GetComponent<ShopItemScript>().Item = item;
            shopItem.GetComponent<ShopItemScript>().InitializationOfItems();
        }
    }

    public void ShowNotificationDialogueBox(InventoryItem Item, bool hasBuyButton = true)
    {
        if (hasBuyButton)
        {
            dialogueMessageText.text = $"Would you buy {Item.ItemName} for {Item.Price} amount of coins?";
            buyItemButton.onClick.RemoveAllListeners();
            buyItemButton.onClick.AddListener(() => BuyItem(Item));

            buyItemButton.gameObject.SetActive(true);
            closeDialogueBoxButton.gameObject.SetActive(true);
            confirmDialogueBoxButton.gameObject.SetActive(false);
        }
        else
        {
            dialogueMessageText.text = "Not enough coins";
            buyItemButton.gameObject.SetActive(false);
            closeDialogueBoxButton.gameObject.SetActive(false);
            confirmDialogueBoxButton.gameObject.SetActive(true);
        }
            
        notificationDialogueBox.SetActive(true);

        
    }

    private void BuyItem(InventoryItem Item)
    {
        string playerID = "Player1";
        MainGameScript.instance.playerScript.Coins -= Item.Price;
        MainGameScript.instance.ApplyPlayerData();

        notificationDialogueBox.SetActive(false);
        Pick(Item, playerID);
    }

    public virtual void Pick(InventoryItem Item, string playerID = "Player1")
    {
        FindTargetInventory(Item.TargetInventoryName, playerID);
        if (_targetInventory == null)
        {
            return;
        }

        if (!Pickable(Item))
        {
            // PickFail();
            return;
        }

        DetermineMaxQuantity(Item);
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
            //Quantity = Quantity - _pickedQuantity;
            //PickSuccess();
            //DisableObjectIfNeeded();
        }
    }

    protected virtual void DetermineMaxQuantity(InventoryItem Item)
    {
        _pickedQuantity = _targetInventory.NumberOfStackableSlots(Item.ItemID, Item.MaximumStack);
        if (Quantity < _pickedQuantity)
        {
            _pickedQuantity = Quantity;
        }
    }
    public virtual bool Pickable(InventoryItem Item)
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
