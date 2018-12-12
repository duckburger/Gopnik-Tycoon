using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfItemSlot : MonoBehaviour
{

    [SerializeField] SpriteRenderer mySpriteRenderer;

    FoodItemData myItem;
    bool isOccupied = false;

    public FoodItemData MyItem
    {
        get
        {
            return this.myItem;
        }
    }

    public bool IsOccupied
    {
        get
        {
            return this.isOccupied;
        }
    }

    private void Start()
    {
        this.mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Populate(FoodItemData item)
    {
        this.myItem = item;
        this.mySpriteRenderer.sprite = item.GetRandomShelfAppearanceSprite();
        this.isOccupied = true;
    }

    public FoodItemData EmptyAndTakeItem()
    {
        if (this.myItem != null)
        {
            FoodItemData itemToTakeOut = this.myItem;
            Empty();
            return itemToTakeOut;
        }
       return null;
    }

    public void Empty()
    {
        this.myItem = null;
        this.mySpriteRenderer.sprite = null;
        this.isOccupied = false;
        Shelf myShelf = GetComponentInParent<Shelf>();
        if (myShelf != null)
        {
            myShelf.DoSlotCheck();
        }
    }


    public void Clear() { }
}
