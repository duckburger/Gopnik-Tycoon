using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingBag : Pickuppable
{
    [SerializeField] List<FoodItemData> allContainedItems = new List<FoodItemData>();

    [SerializeField] SpriteRenderer bagSpriteRenderer;
    [SerializeField] SpriteRenderer contentsSpriteRenderer;

    public SpriteRenderer BagSpriteRenderer
    {
        get
        {
            return this.contentsSpriteRenderer;
        }
    }

    public void PopulateNewBag(List<FoodItemData> newItems)
    {
        // Creating the bag
        this.allContainedItems.AddRange(newItems);
        UpdateBagAppearance();
    }

    private void UpdateBagAppearance()
    {
        int itemCount = this.allContainedItems.Count;
        if (itemCount < 2)
        {
            this.contentsSpriteRenderer.sprite = ItemLibrary.Instance.shoppingBagContentsLow;
        }
        else if (itemCount < 8)
        {
            this.contentsSpriteRenderer.sprite = ItemLibrary.Instance.shoppingBagContentsMed;
        }
        else if (itemCount < 20)
        {
            this.contentsSpriteRenderer.sprite = ItemLibrary.Instance.shoppingBagContentsHigh;
        }
    }

    public void AddToBag(FoodItemData itemToAdd)
    {
        this.allContainedItems.Add(itemToAdd);
        UpdateBagAppearance();
    }

    public void RemoveFromBag(FoodItemData itemToRemove)
    {
        if (this.allContainedItems.Contains(itemToRemove))
        {
            this.allContainedItems.Remove(itemToRemove);
            UpdateBagAppearance();
        }
    }

    public void Clear()
    {

    }
}
