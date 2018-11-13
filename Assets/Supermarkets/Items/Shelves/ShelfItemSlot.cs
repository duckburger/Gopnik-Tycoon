using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfItemSlot : MonoBehaviour
{

    [SerializeField] SpriteRenderer mySpriteRenderer;

    FoodQuality myItemQuality;
    public FoodQuality MyItemQuality
    {
        get
        {
            return this.myItemQuality;
        }
    }

    FoodItem myItem;
    bool isOccupied = false;
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

    public void Populate(FoodItem item)
    {
        this.myItem = item;
        this.mySpriteRenderer.sprite = item.GetRandomShelfAppearanceSprite();
        this.myItemQuality = item.FoodQuality;
        this.isOccupied = true;
    }

    public FoodItem EmptyAndTakeItem()
    {
        if (this.myItem != null)
        {
            FoodItem itemToTakeOut = this.myItem;
            Empty();
            return itemToTakeOut;
        }
       return null;
    }

    public void Empty()
    {
        this.myItem = null;
        this.mySpriteRenderer.sprite = null;
        this.myItemQuality = FoodQuality.None;
        this.isOccupied = false;
    }


    public void Clear() { }
}
