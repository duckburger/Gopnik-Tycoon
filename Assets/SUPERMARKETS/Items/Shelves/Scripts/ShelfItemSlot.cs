using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfItemSlot : MonoBehaviour
{
    [SerializeField] SpriteRenderer mySpriteRenderer;
    [SerializeField] SpriteRenderer priceTagSpriteRenderer;

    FoodItemData myItem;
    bool isOccupied = false;

    public FoodItemData MyItem => this.myItem;
    public bool IsOccupied => this.isOccupied;

    private void Start()
    {
        this.mySpriteRenderer = GetComponent<SpriteRenderer>();
        if (this.transform.childCount > 0)
        {
            this.priceTagSpriteRenderer = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
    }

    public void Populate(FoodItemData item)
    {
        this.myItem = item;
        this.mySpriteRenderer.sprite = item.GetRandomShelfAppearanceSprite();
        this.priceTagSpriteRenderer.enabled = true;
        this.priceTagSpriteRenderer.sprite = BuildingTracker.Instance.GetRandomPriceTagSprite();
        this.isOccupied = true;
    }

    public FoodItemData EmptyAndTakeItem(bool stealItem = false)
    {
        if (this.myItem != null)
        {
            FoodItemData itemToTakeOut = this.myItem;
            Empty(stealItem);
            return itemToTakeOut;
        }
       return null;
    }

    public void Empty(bool stealItem = false)
    {
        this.myItem = null;
        this.mySpriteRenderer.sprite = null;
        if (!stealItem)
        {
            this.priceTagSpriteRenderer.sprite = null;
            this.priceTagSpriteRenderer.enabled = false;
        }
        this.isOccupied = false;
        Shelf myShelf = GetComponentInParent<Shelf>();
        if (myShelf != null)
        {
            myShelf.DoSlotCheck();
        }
    }


    public void Clear() { }
}
