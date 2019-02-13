using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MCharCarry : MonoBehaviour
{
    [SerializeField] Transform carryTransform;
    [SerializeField] Pickuppable currentItem;

    bool isCarryingShoppingBag = false;

    bool isPlayer = false;
    public Pickuppable CurrentItem
    {
        get
        {
            return this.currentItem;
        }
    }

    List<Pickuppable> nearItems = new List<Pickuppable>();


    private void Start()
    {
        if (this.gameObject.tag == "Player")
        {
            this.isPlayer = true;
        }
    }

    
    public float GetCostOfCarriedGoods()
    {
        if (!this.isCarryingShoppingBag)
        {
            Debug.LogError("Trying to get value of non food items (character " + this.gameObject.name + " has no shopping bag");
            return -1;
        }

        float cost = -1;
        ShoppingBag bag = this.currentItem.GetComponent<ShoppingBag>();
        if (bag != null)
        {
            cost = bag.GetCostOfContainedGoods();
        }
        return cost;
    }

    public int GetCountOfCarriedGoods()
    {
        if (!this.isCarryingShoppingBag && this.currentItem != null)
        {
            return 1;
        }
        if (this.currentItem == null)
        {
            return 0;
        }
        ShoppingBag bag = this.currentItem.GetComponent<ShoppingBag>();
        if (bag != null)
        {
            return bag.AllContainedItems.Count;
        }
        return 0;
    }

    #region Adding/Removing items from field of view

    public void AddToNearItem(Pickuppable newItem)
    {
        if (!this.nearItems.Contains(newItem))
        {
            this.nearItems.Add(newItem);
        }
    }

    public void RemoveNearItem(Pickuppable itemToRemove)
    {
        if (this.nearItems.Contains(itemToRemove))
        {
            this.nearItems.Remove(itemToRemove);
        }
    }

    #endregion

    Pickuppable FindNearestItem()
    {
        float nearestDist = Mathf.Infinity;
        int index = 0;
        for (int i = 0; i < nearItems.Count; i++)
        {
            float distTo = Vector2.Distance(this.transform.position, nearItems[i].transform.position);
            if (distTo < nearestDist)
            {
                nearestDist = distTo;
                index = i;
            }
        }
        if (nearItems == null || nearItems.Count <= 0)
        {
            return null;
        }
        return nearItems[index];
    }

    private void Update()
    {
        if (this.isPlayer && Input.GetKeyDown(KeyCode.E))
        {
            PickUpNearestItem(); 
        }
        if (this.isPlayer && ExternalPlayerController.Instance.PlayerCarryController.currentItem != null && Input.GetKeyDown(KeyCode.G))
        {
            DropItem();
        }
    }

    public void PickUpNearestItem()
    {
        Pickuppable itemToPickup = FindNearestItem();
        if (this.currentItem != null)
        {
            DropItem(); 

        }
        if (this.nearItems.Count <= 0)
        {
            Debug.Log("No pickuppable items near the player");
            return;
        }
        Debug.Log("Picking up an item");
       
        if (nearItems.Count <= 0)
        {
            return;
        }

        itemToPickup.transform.parent = this.carryTransform;
        this.currentItem = itemToPickup;
        this.currentItem.transform.localPosition = Vector2.zero;
    }

    public void PickUpSpecificItem(PickuppableItemData itemToPickUp)
    {
        if (itemToPickUp == null)
        {
            Debug.Log("No pickuppable item specified");
            return;
        }

        if (this.currentItem != null)
        {
            DropItem();
        }

        //generatedGeneric.transform.parent = this.carryTransform;
        // this.currentItem =
        this.currentItem.transform.localPosition = Vector2.zero;
    }

    public void PickUpFoodItem(FoodItemData foodItemToPickUp)
    {
        if (foodItemToPickUp == null)
        {
            Debug.Log("No pickuppable item specified");
            return;
        }

        if (this.currentItem != null)
        {
            if (this.isCarryingShoppingBag)
            {
                ShoppingBag bag = this.currentItem.gameObject.GetComponent<ShoppingBag>();
                bag.AddToBag(foodItemToPickUp);
                return;
            }
            else
            {
                DropItem();
                CreateNewBagWithItem(foodItemToPickUp);
                this.isCarryingShoppingBag = true;
                return;
            }

        }

        CreateNewBagWithItem(foodItemToPickUp);
        this.isCarryingShoppingBag = true;
        this.currentItem.transform.localPosition = Vector2.zero;
    }

    private void CreateNewBagWithItem(FoodItemData foodItemToPickUp)
    {
        GameObject newBag = ItemLibrary.Instance.CreateShoppingBag();
        newBag.transform.SetParent(this.carryTransform);
        ShoppingBag bagScript = newBag.GetComponent<ShoppingBag>();
        List<FoodItemData> itemList = new List<FoodItemData>();
        itemList.Add(foodItemToPickUp);
        bagScript.PopulateNewBag(itemList);
        this.currentItem = bagScript;
        this.currentItem.transform.localPosition = Vector2.zero;
    }

    public void DropItem()
    {
        if (this.currentItem != null)
        {
            this.currentItem.transform.parent = LevelData.CurrentLevel.Floor;
            this.currentItem.transform.position = this.transform.position;
            this.currentItem = null;
            this.isCarryingShoppingBag = false;
        }
    }

}
